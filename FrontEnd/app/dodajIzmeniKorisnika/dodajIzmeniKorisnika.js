
'use strict'
class Korisnik {
    constructor(id, korisnickoIme, ime, prezime, datumRodjenja,grupeKorisnika) {
        this.id = id
        this.korisnickoIme = korisnickoIme
        this.ime = ime
        this.prezime = prezime
        this.datumRodjenja = datumRodjenja
        this.grupeKorisnika = grupeKorisnika
    }
}


//postavi prazne metode i url
let method=``
let url=``
let success = ""
//formatiraj pravilan datum za formu
function formatDate(isoDateString) {
  const date = new Date(isoDateString)

  const year = date.getFullYear()
  const month = String(date.getMonth() + 1).padStart(2, '0') // Months are 0-based
  const day = String(date.getDate()).padStart(2, '0')

  return `${year}-${month}-${day}`
}


function get() {
  const urlParams = new URLSearchParams(window.location.search)
  const id = urlParams.get('korisnikId') // Preuzimamo vrednost korisnikId parametra upita

if (!id) { // Ako ne postoji parametar upita, forma je prazna za unos novog korisnika i POST
    method = `POST`
    url = `http://localhost:14117/api/korisnik`
    success = "upis"
}else{//ako nije prazan menja metod i url za PUT 
    method = `PUT`
    url = `http://localhost:14117/api/korisnik/${id}`
    success = "izmena"

    fetch(url)
        .then(response => {
        if (!response.ok) {
            // Ako statusni kod nije iz 2xx (npr. 404), kreiramo grešku
            const error = new Error('Zahtev neuspesan. Status: ' + response.status)
            error.response = response // Dodajemo ceo response objekat u grešku
            throw error // Bacamo grešku
        }
        return response.json()
        })
        .then(korisnik => { // Ako je zahtev uspešan, popunjavamo polja sa podacima
        document.querySelector('#korisnickoIme').value = korisnik.korisnickoIme
        document.querySelector('#ime').value = korisnik.ime
        document.querySelector('#prezime').value = korisnik.prezime
        document.querySelector('#datumRodjenja').value = formatDate(korisnik.datumRodjenja)
        })
        .catch(error => {
        console.error('Error:', error.message)
        if (error.response && error.response.status === 404) {
            alert('Korisnik ne postoji!')
        } else {
            alert('Desila se greska pokusajte ponovo!')
        }
        })
    }
}
    


let submitBtn = document.querySelector("#form-submit-Btn")
    submitBtn.addEventListener("click", function(event){
        event.preventDefault()
        const form = document.querySelector('#form-data')
        let data = new FormData(form)

        const reqBody = {
        korisnickoIme: data.get('korisnickoIme'),
        ime: data.get('ime'),
        prezime: data.get('prezime'),
        datumRodjenja: data.get('datumRodjenja')
            }
            //Provera forme
            let errorMsg = document.querySelector("#error-msg")
                if (reqBody.korisnickoIme.trim() === '') { 
                errorMsg.textContent = 'Korisnicko ime je obavezno polje'
                return
            }
            
                if (reqBody.ime.trim() === '') { 
                errorMsg.textContent = 'Ime je obavezno polje'
                return
            }
            
                if (reqBody.prezime.trim() === '') { 
                errorMsg.textContent = 'Prezime je obavezno polje'
                return
            }
            
                if (reqBody.datumRodjenja.trim() === '') { 
                errorMsg.textContent = 'Datum rodjenja je obavezno polje'
                return
            }
            
            fetch(url, { // Pravi POST zahtev da se sačuva korisnika
                method: method,
                headers: {
                'Content-Type': 'application/json'
                },
                body: JSON.stringify(reqBody)
            })
                .then(response => {
                if (!response.ok) {
                    // Ako statusni kod nije iz 2xx (npr. 400), kreiramo grešku
                    const error = new Error('Request failed. Status: ' + response.status)
                    error.response = response // Dodajemo ceo response objekat u grešku
                    throw error  // Bacamo grešku
                }
                return response.json()
                })
                .then(data => {
                window.location.href = `../pregledKorisnika/pregledKorisnika.html?response=${success}`
                })
                .catch(error => {
                console.error('Error:', error.message)
                if(error.response && error.response.status === 400) {
                    alert('Data is invalid!')
                }
                else {
                    alert('An error occurred while updating the data. Please try again.')
                }
        })
    })



document.addEventListener('DOMContentLoaded', get)













