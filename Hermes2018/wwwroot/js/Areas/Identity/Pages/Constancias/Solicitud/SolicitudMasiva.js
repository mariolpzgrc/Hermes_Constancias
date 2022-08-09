

function agregarFila() {
    var tableBody = document.getElementById("cuerpoTablaSolicitudMasiva");
    var rowCount = tableBody.rows.length;

    if (rowCount < 30) {
        console.log(rowCount);
        tableBody.insertRow(-1).innerHTML = '<td contenteditable=true class="celda-editable solo-numeros"></td><td></td><td contenteditable=true class="celda-editable solo-numeros"></td><td contenteditable=true class="celda-editable solo-numeros"></td><td></td><td contenteditable=true class="celda-editable"></td><td contenteditable=true class="celda-editable solo-numeros"></td><td></td>';
    }
}

function eliminarFila() {
    var table = document.getElementById("cuerpoTablaSolicitudMasiva");
    var rows = table.rows.length;
    if (rows < 1) {
        alert('No se puede eliminar el encabezado.')
    } else {
        table.deleteRow(rows - 1);
    }
}

$(".solo-numeros").keypress(function (e) {
    if (isNaN(String.fromCharCode(e.which))) e.preventDefault();
});