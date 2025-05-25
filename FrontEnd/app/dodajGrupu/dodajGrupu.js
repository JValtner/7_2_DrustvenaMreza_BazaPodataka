
'use strict'
class Group {
    constructor(id, ime, datumOsnivanja) {
        this.id = id        
        this.ime = ime        
        this.datumOsnivanja = datumOsnivanja        
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
    method = `POST`
    url = `http://localhost:14117/api/groups`
    success = "upis"   

    let submitBtn = document.querySelector("#form-submit-Btn")
    submitBtn.addEventListener("click", function(event){
        event.preventDefault()
        const form = document.querySelector('#form-data')
        let data = new FormData(form)

        const reqBody = {
        ime: data.get('nazivGrupe'),
        datumOsnivanja: data.get('datumOsnivanja')
            }
            //Provera forme
            let errorMsg = document.querySelector("#error-msg")
                if ( reqBody.ime.trim() === '') { 
                errorMsg.textContent = 'Naziv Grupe je obavezno polje'
                return
            }
                if (reqBody.datumOsnivanja.trim() === '') { 
                errorMsg.textContent = 'Datum osnivanja je obavezno polje'
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
                window.location.href = `../pregledGrupa/pregledGrupa.html?response=${success}`
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



}
document.addEventListener('DOMContentLoaded', get)