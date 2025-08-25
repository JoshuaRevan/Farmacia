const MODELO_BASE = {
    idPresentacion: 0,
    tipoPresentacion: "",
};

let tablaData;

$(function () {

    // Reinicia DataTable si ya existe
    if ($.fn.DataTable.isDataTable('#tbdata')) {
        $('#tbdata').DataTable().destroy();
    }

    // Inicializar DataTable
    tablaData = $('#tbdata').DataTable({
        responsive: true,
        ajax: {
            url: '/Presentaciones/Lista',
            type: 'GET',
            datatype: 'json',
            dataSrc: 'data'
        },
        columns: [
            { data: "idPresentacion", visible: false, searchable: false },
            { data: "tipoPresentacion", title: "tipoPresentacion" },
           
            {
                defaultContent: `
                    <button class="btn btn-primary btn-editar btn-sm mr-2">
                        <i class="fas fa-pencil-alt"></i>
                    </button>
                    <button class="btn btn-danger btn-eliminar btn-sm">
                        <i class="fas fa-trash-alt"></i>
                    </button>`,
                orderable: false,
                searchable: false,
                width: "80px"
            }
        ],
        order: [[0, "desc"]],
        dom: "Bfrtip",
        buttons: [
            {
                text: 'Exportar Excel',
                extend: 'excelHtml5',
                title: '',
                filename: 'Reporte Presentaciones',
                exportOptions: {
                    columns: [1] // Columnas visibles
                }
            },
            'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
});

function mostrarModal(modelo = MODELO_BASE) {
    $("#txtId").val(modelo.idPresentacion);
    $("#txtDescripcion").val(modelo.tipoPresentacion);
   
    $("#modalData").modal("show");
}

$("#btnNuevo").click(function () {
    mostrarModal();
});

$("#btnGuardar").click(function () {

    /*debugger;*/

    if  ($("#txtDescripcion").val().trim() == "") {
        toastr.warning("", "debe de completar el campo")
        $("#txtDescripcion").focus()
        return;
    }

    const modelo = structuredClone(MODELO_BASE);
    modelo["idPresentacion"] = parseInt($("#txtId").val())
    modelo["tipoPresentacion"] = $("#txtDescripcion").val()
    

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.idPresentacion == 0) {
        fetch("/Presentaciones/Crear", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {
                if (responseJson.estado) {
                    tablaData.row.add(responseJson.objeto).draw(false);
                    $("#modalData").modal("hide");

                    
                    swal({
                        title: "Presentacion creado exitosamente",
                        
                        icon: "success",
                        button: "entendido"
                    });

                } else {
                    swal("Lo sentimos!", responseJson.mensaje, "error");
                }
            })
            .catch(error => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                console.error("Error al crear Presentacion:", error);
                swal("Error", "No se pudo crear la Presentacion", "error");
            });

    } else {
        fetch("/Presentaciones/Editar", {
            method: "PUT",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(modelo)
        })
            .then(response => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                return response.ok ? response.json() : Promise.reject(response);
            })
            .then(responseJson => {
                if (responseJson.estado) {

                    tablaData.row(filaSeleccionada).data(responseJson.objeto).draw(false);
                    filaSeleccionada = null;
                    $("#modalData").modal("hide")
                    swal("Listo!", "La Presentacion fue modificado correctamente", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    }
});

let filaSeleccionada;
$("#tbdata tbody").on("click", ".btn-editar", function () {
    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();

    mostrarModal(data);
})

$("#tbdata tbody").on("click", ".btn-eliminar", function () {
    let fila;
    if ($(this).closest("tr").hasClass("child")) {
        fila = $(this).closest("tr").prev();
    } else {
        fila = $(this).closest("tr");
    }

    const data = tablaData.row(fila).data();

    swal({
        title: "¿Estás seguro de eliminar?",
        text: `Eliminar Presentacion "${data.Presentaciones}"`, // revisar Descripcion y presnetacion para ver si coincide 
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Sí, eliminar",
        cancelButtonText: "No, cancelar",
        closeOnConfirm: false
    }, function (respuesta) {
        if (respuesta) {
            $(".showSweetAlert").LoadingOverlay("show");

            $.ajax({
                url: `/Presentaciones/Eliminar?idPresentacion=${data.idPresentacion}`,
                type: "DELETE",
                dataType: "json",
                success: function (responseJson) {
                    if (responseJson.estado) {
                        tablaData.row(fila).remove().draw();
                        swal("¡Listo!", "La Presentacion fue eliminado", "success");
                    } else {
                        swal("Lo sentimos", responseJson.mensaje, "error");
                    }
                },
                error: function () {
                    swal("Error", "No se pudo conectar al servidor", "error");
                },
                complete: function () {
                    $(".showSweetAlert").LoadingOverlay("hide");
                }
            });
        }
    });
});


