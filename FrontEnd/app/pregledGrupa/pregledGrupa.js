
'use strict'
class Group {
    constructor(id, ime, datumOsnivanja) {
        this.id = id        
        this.ime = ime        
        this.datumOsnivanja = datumOsnivanja        
    }
}

const urlParams = new URLSearchParams(window.location.search)
const response = urlParams.get('response') // Preuzimamo vrednost response parametra upita


function initializeGroup() {
    let grupe = []
    
    grupe = loadGroups()
    
    saveLocalStorage(grupe)
    showSuccess()
}

function saveLocalStorage(grupe) {
    let grupeJSON = JSON.stringify(grupe)
    localStorage.setItem("grupe", grupeJSON)
}

function loadGroups(){
  fetch('http://localhost:14117/api/groups') 
    .then(response => {
      if (!response.ok) {
        throw new Error('Request failed. Status: ' + response.status)
      }
      return response.json()
    })
    .then(grupe => createDataTable(grupe))  
    .catch(error => {                  
      console.error('Error:', error.message)
      alert('An error occurred while loading the data. Please try again.')
    })
}

function createDataTable(grupe) {
    let container = document.querySelector(".main-content") 
    container.innerHTML = `
        <table class="user-data">
            <thead class="user-data-head">
                <tr>
                    <th>Id</th>             
                    <th>Naziv Grupe</th>        
                    <th>Datum rodjenja</th>                     
                    <th>Brisanje</th> 
                </tr>
            </thead>
            <tbody id="user-data-body">
            </tbody>
        </table>
    `

    const tbody = container.querySelector("#user-data-body")

    for (let grupa of grupe) {
        const row = document.createElement("tr")
        row.innerHTML = `
            <td>${grupa.id}</td>         
            <td>${grupa.ime}</td>             
            <td>${formatDate(grupa.datumOsnivanja)}</td>
            <td><button id="ObrisiGrupu">Obrisi</button> <button id="prikaziKorisnike">Prikazi korisnike</button></td>
        `
        let button = row.querySelector("#ObrisiGrupu")
        button.addEventListener("click", function () {
            fetch('http://localhost:14117/api/groups/' + grupa.id, {method: 'DELETE'})
            .then(response =>{
                if (!response.ok) {
                const error = new Error('Request failed. Status: ' + response.status)
                error.response = response 
                throw error  
            }
            loadGroups()
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

        let prikazBtn = row.querySelector("#prikaziKorisnike")
        prikazBtn.addEventListener("click", function(){
            window.location.href=`../pregledKorisnikaGrupe/pregledKorisnikaGrupe.html?grupaId=${grupa.id}&grupaIme=${grupa.ime}`
        })

        tbody.appendChild(row)
    }
}

function formatDate(isoDateString) {
    const date = new Date(isoDateString)
    return date.toLocaleDateString('sr-RS')
}

let dodajBtn = document.querySelector("#dodajGrupuBtn")
dodajBtn.addEventListener("click", function(event){
    window.location.href="../dodajGrupu/dodajGrupu.html"
})

function showSuccess() {
    let successMsg = document.querySelector("#success-msg")
    if (response==="upis"){
        successMsg.textContent = "Grupa je uspešno dodata"
    }   
    successMsg.style.opacity = "1"
    successMsg.style.color = "green"
    successMsg.style.fontWeight = "bold"

    setTimeout(() => {
        successMsg.style.opacity = "0"
    }, 3000)
}

document.addEventListener('DOMContentLoaded', initializeGroup)