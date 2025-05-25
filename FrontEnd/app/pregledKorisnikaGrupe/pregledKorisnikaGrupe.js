'use strict'
class Korisnik {
    constructor(id, korisnickoIme, ime, prezime, datumRodjenja, grupeKorisnika) {
        this.id = id
        this.korisnickoIme = korisnickoIme
        this.ime = ime
        this.prezime = prezime
        this.datumRodjenja = datumRodjenja
        this.grupeKorisnika = grupeKorisnika
    }
}
class Grupa{
    constructor(id,ime,datumOsnivanja){
        this.id =id
        this.ime = ime
        this.datumOsnivanja = datumOsnivanja
    }
}

const urlParams = new URLSearchParams(window.location.search)
const grupaId = [urlParams.get('grupaId')] // Preuzimamo vrednost grupaId parametra upita
const grupaIme = urlParams.get('grupaIme')//Preuzimamo vrednost grupaime parametra upita

function loadKorisnikeGrupe(){
  fetch(`http://localhost:14117/api/groups/${grupaId}`) 
    .then(response => {
      if (!response.ok) {
        throw new Error('Request failed. Status: ' + response.status)
      }
      return response.json()
    })
    .then(korisniciGrupe => createDataTable(korisniciGrupe))
    .then(loadKorisnikaBezGrupe())  
    .catch(error => {                  
      console.error('Error:', error.message)
      alert('An error occurred while loading the data. Please try again.')
    })
}
function createDataTable(korisniciGrupe) {
    let grupaNaziv = document.querySelector("#naziv-grupe")
    grupaNaziv.textContent = grupaIme     // postavljanje teksta

    let container = document.querySelector(".main-content") 

    if (!korisniciGrupe || korisniciGrupe.length === 0) {
        container.innerHTML = `<p> U grupi ne postoji ni jedan korisnik</p>`
        return    
    }
    
    container.innerHTML = `
        <th>
        <table class="user-data">
            <thead class="user-data-head">
                <tr>
                    <th>Id</th> 
                    <th>Korisničko Ime</th> 
                    <th>Ime</th> 
                    <th>Prezime</th> 
                    <th>Datum rodjenja</th>
                    <th>Izbaci</th> 
                </tr>
            </thead>
            <tbody id="user-data-body">
            </tbody>
        </table>
    `
    const tbody = container.querySelector("#user-data-body")

    for (let korisnik of korisniciGrupe) {
        const row = document.createElement("tr")
        row.innerHTML = `
            <td>${korisnik.id}</td>
            <td>${korisnik.korisnickoIme}</td>
            <td>${korisnik.ime}</td>
            <td>${korisnik.prezime}</td>
            <td>${formatDate(korisnik.datumRodjenja)}</td>
            <td><button id="izbaciIzGrupe">-</button></td>
            `
        let button = row.querySelector("#izbaciIzGrupe")
        button.addEventListener("click", function () {
            fetch(`http://localhost:14117/api/korisnik/${korisnik.id}/grupa/` + grupaId, {method: 'DELETE'})
            .then(response =>{
                if (!response.ok) {
                const error = new Error('Request failed. Status: ' + response.status)
                error.response = response 
                throw error  
            }
            loadKorisnikeGrupe()
            loadKorisnikaBezGrupe()
            })
            .catch(error =>{
                console.error('Error: ' +error.message)
                if(error.response && error,response.status === 404){
                    alert('Group does not exist')
                } else {
                    alert('An error occured while deleting the the group. Please try again.')
                }
                    
            })
        })
            

        tbody.appendChild(row)
    }
}

function loadKorisnikaBezGrupe(){
  fetch(`http://localhost:14117/api/korisnik`) 
    .then(response => {
      if (!response.ok) {
        throw new Error('Request failed. Status: ' + response.status)
      }
      return response.json()
    })
    .then(korisniciGrupe2 => createDataTable2(korisniciGrupe2))  
    .catch(error => {                  
      console.error('Error:', error.message)
      alert('An error occurred while loading the data. Please try again.')
    })
}

function createDataTable2(korisniciGrupe) {

    let container = document.querySelector(".sec-content") 

    if (!korisniciGrupe || korisniciGrupe.length === 0) {
        container.innerHTML = `<p> U grupi ne postoji ni jedan korisnik</p>`
        return    
    }
    
    container.innerHTML = `
        <th>
        <table class="user-data">
            <thead class="user-data-head">
                <tr>
                    <th>Id</th> 
                    <th>Korisničko Ime</th> 
                    <th>Ime</th> 
                    <th>Prezime</th> 
                    <th>Datum rodjenja</th> 
                    <th>Dodaj</th>
                </tr>
            </thead>
            <tbody id="user-data-body">
            </tbody>
        </table>
    `
    const tbody = container.querySelector("#user-data-body")

    for (let korisnik of korisniciGrupe) {
        if(korisnik.grupeKorisnika == ""){
            const row = document.createElement("tr")
            row.innerHTML = `
            <td>${korisnik.id}</td>
            <td>${korisnik.korisnickoIme}</td>
            <td>${korisnik.ime}</td>
            <td>${korisnik.prezime}</td>
            <td>${formatDate(korisnik.datumRodjenja)}</td>
            <td><button id="dodajUGrupu">+</button></td>
            `
        let button = row.querySelector("#dodajUGrupu")
        button.addEventListener("click", function () {
            fetch(`http://localhost:14117/api/korisnik/${korisnik.id}/grupa/` + grupaId, {method: 'POST'})
            .then(response =>{
                if (!response.ok) {
                const error = new Error('Request failed. Status: ' + response.status)
                error.response = response 
                throw error  
            }
            loadKorisnikeGrupe()
            loadKorisnikaBezGrupe()
            })
            .catch(error =>{
                console.error('Error: ' +error.message)
                if(error.response && error,response.status === 404){
                    alert('Group does not exist')
                } else {
                    alert('An error occured while deleting the the group. Please try again.')
                }
                    
            })
        })
            tbody.appendChild(row)
        }
        
    }
}


function formatDate(isoDateString) {
    const date = new Date(isoDateString)
    return date.toLocaleDateString('sr-RS')
}

document.addEventListener('DOMContentLoaded', loadKorisnikeGrupe)