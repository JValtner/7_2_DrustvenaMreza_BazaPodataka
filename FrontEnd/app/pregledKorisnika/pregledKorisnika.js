
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

const urlParams = new URLSearchParams(window.location.search)
const response = urlParams.get('response') // Preuzimamo vrednost response parametra upita


function initializeKorisnike() {
    let korisnici = []
    
    korisnici = loadKorisnike()
    
    saveLocalStorage(korisnici)
    showSuccess()
}

function saveLocalStorage(korisnici) {
    let korisniciJSON = JSON.stringify(korisnici)
    localStorage.setItem("korisnici", korisniciJSON)
}
function loadKorisnike(){
  fetch('http://localhost:14117/api/korisnik') 
    .then(response => {
      if (!response.ok) {
        throw new Error('Request failed. Status: ' + response.status)
      }
      return response.json()
    })
    .then(korisnici => createDataTable(korisnici))  
    .catch(error => {                  
      console.error('Error:', error.message)
      alert('An error occurred while loading the data. Please try again.')
    })
}
function createDataTable(korisnici) {
    let container = document.querySelector(".main-content") 
    container.innerHTML = `
        <table class="user-data">
            <thead class="user-data-head">
                <tr>
                    <th>Id</th> 
                    <th>Korisničko Ime</th> 
                    <th>Ime</th> 
                    <th>Prezime</th> 
                    <th>Datum rodjenja</th> 
                    <th>Opcije</th> 
                </tr>
            </thead>
            <tbody id="user-data-body">
            </tbody>
        </table>
    `

    const tbody = container.querySelector("#user-data-body")

    for (let korisnik of korisnici) {
        const row = document.createElement("tr")
        row.innerHTML = `
            <td>${korisnik.id}</td>
            <td>${korisnik.korisnickoIme}</td>
            <td>${korisnik.ime}</td>
            <td>${korisnik.prezime}</td>
            <td>${formatDate(korisnik.datumRodjenja)}</td>
            <td><button id="izmeniKorisnikaBtn">Izmeni podatke</button></td>
        `
        let button = row.querySelector("#izmeniKorisnikaBtn")
        button.addEventListener("click", function () {
            window.location.href = `../dodajIzmeniKorisnika/dodajIzmeniKorisnika.html?korisnikId=${korisnik.id}`
        })
        tbody.appendChild(row)
    }
}
function formatDate(isoDateString) {
    const date = new Date(isoDateString)
    return date.toLocaleDateString('sr-RS')
}

//event listener za dodaj Btn
let dodajBtn = document.querySelector("#dodajKorisnikaBtn")
dodajBtn.addEventListener("click", function(event){
    window.location.href="../dodajIzmeniKorisnika/dodajIzmeniKorisnika.html"
})

function showSuccess() {
    let successMsg = document.querySelector("#success-msg")
    if (response==="upis"){
        successMsg.textContent = "Korisnik je uspešno dodat"
    }
    if (response==="izmena"){
        successMsg.textContent = "Korisnik je uspešno izmenjen"
    }
    successMsg.style.opacity = "1"
    successMsg.style.color = "green"
    successMsg.style.fontWeight = "bold"

    setTimeout(() => {
        successMsg.style.opacity = "0"
    }, 3000)
}

document.addEventListener('DOMContentLoaded', initializeKorisnike)