const MODELO_BASE = {
    idMarca: 0,
    nombreMarca: "",
};

let tablaData; 

$(function () {
    if ($.fn.DataTable.isDataTable('#tbdata')) {
        $('#tbdata').DataTable().destroy();
    }

    tablaData = $('#tbdata').DataTable({
        responsive: true,
        ajax: {
            url: '/Marcas/Lista',
            type: 'GET',
            datatype: 'json',
            datasrc: 'data'
        },
        columns: [
            { data: "idMarca", visible: false, searchable: false },
            { data: "nombreMarca", title: "nombreMarca" },
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
    $("#txtId").val(modelo.idMarca);
    $("#txtDescripcion").val(modelo.nombreMarca);

    $("#modalData").modal("show");

}

$("#btnNuevo").on('click', function () {
    mostrarModal();
});

$('#btnGuardar').click(function () {
    if ($("#txtDescripcion").val().trim() == "") {
        toastr.warning("", "debe de completar el campo")
        $("#txtDescripcion").focus()
        return;
    }

    const modelo = structuredClone(MODELO_BASE);
    modelo["idMarca"] = parseInt($("#txtId").val())
    modelo["nombreMarca"] = $("#txtDescripcion").val()

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.idMarca == 0) {
        fetch("/Marcas/Crear", {
            method: "POST",
            headers: {
                "Content-type": "application/json"
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
                        title: "Marca creada exitosamente",
                        icon: "success",
                        button: "entendido"
                    });
                } else {
                    swal("lo sentimos!", responseJson.mensaje, "error");
                }
            })
            .catch(error => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                console.error("error al crear la marca", error);
                swal("Error", "No se puede crear la marca", "error");

            });
    } else {
        fetch("/Marcas/Editar", {
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
                    swal("Listo!", "la Marca fue modificada correctamente", "success")
                } else {
                    swal("lo sentimos", responseJson.mensaje, "error")
                }
            })
    }
});

let filaSeleccionada;
$("#tbdata tbody").on("click", ".btn-editar", function () {
    let filaSeleccionada;

    if ($(this).closest("tr").hasClass("child")) {
        filaSeleccionada = $(this).closest("tr").prev();
    } else {
        filaSeleccionada = $(this).closest("tr");
    }

    const data = tablaData.row(filaSeleccionada).data();
    mostrarModal(data);
});

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
        text: `Eliminar Marca "${data.nombreMarca}"`, // revisar Descripcion y presnetacion para ver si coincide 
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
                url: `/Marcas/Eliminar?idMarca=${data.idMarca}`,
                type: "DELETE",
                dataType: "json",
                success: function (responseJson) {
                    if (responseJson.estado) {
                        tablaData.row(fila).remove().draw();
                        swal("¡Listo!", "La Marca fue eliminado", "success");
                    } else {
                        swal("Lo sentimos", responseJson.mensaje, "error");
                    }
                },
                error: function () {
                    swal("Error", "No se puede conectar al servidor", "error");
                },
                complete: function () {
                    $(".showSweetAlert").LoadingOverlay("hide");
                }
            });
        }
    });
});