const MODELO_BASE = {
    idProveedor: 0,
    nombreProveedor: "",
    telefonoProveedor: ""
};

let tablaData;
$(function () {
    if ($.fn.DataTable.isDataTable('#tbdata')) {
        $('#tbdata').DataTable().destroy();
    }

    tablaData = $('#tbdata').DataTable({
        responsive: true,
        ajax: {
            url: '/Proveedores/Lista',
            type: 'GET',
            datatype: 'json',
            dataSrc: 'data'
        },
        columns: [
            { data: "idProveedor", visible: false, searchable: false },
            { data: "nombreProveedor", title: "nombreProveedor" },
            { data: "telefonoProveedor", title: "telefonoProveedor" },
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
                filename: 'Reporte Proveedores',
                exportOptions: {
                    colums: [2] //culumnas visibles 
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
    $("#txtId").val(modelo.idProveedor);
    $("#txtNombre").val(modelo.nombreProveedor);
    $("#txtTelefono").val(modelo.telefonoProveedor);

    $("#modalData").modal("show");
}

$("#btnNuevo").on('click', function () {
    mostrarModal();
});

$("#btnGuardar").on('click', function () {
    if ($("#txtNombre").val().trim() == "") {
        toastr.warning("", "debe de complementar el campo")
        $("#txtNombre").focus();
        return;
    }
    if ($("#txtTelefono").val().trim() == "") {
        toastr.warning("", "debe de complementar el campo")
        $("#txtTelefono").focus();
        return;
    }

    const modelo = structuredClone(MODELO_BASE);
    modelo["idProveedor"] = parseInt($("#txtId").val())
    modelo["nombreProveedor"] = $("#txtNombre").val()
    modelo["telefonoProveedor"] = $("#txtTelefono").val()

    $("#modalData").find("modal-content").LoadingOverlay("show");

    if (modelo.idProveedor == 0) {
        fetch("/Proveedores/Crear", {
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
                        title: "Proveedor creado exitosamente",

                        icon: "succes",
                        button: "entendido"
                    });

                } else {
                    swal("Lo sentimos!", responseJson.mensaje, "error");
                }
            })
            .catch(error => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                console.error("Error al crear Proveedor:", error);
                swal("Error", "No se puede crear la presentacion", "error");
            });
    } else {
        fetch("/Proveedores/Editar", {
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
                    swal("Listo!", "El proveedor fue modificado correctamente", "success")

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
        Text: `Eliminar Proveedor "${data.Proveedores}"`,
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
                url: `/Proveedores/Eliminar?idProveedor=${data.idProveedor}`,
                type: "DELETE",
                dataType: "json",
                success: function (responseJson) {
                    if (responseJson.estado) {
                        tablaData.row(fila).remove().draw();
                        swal("¡Listo!", "El proveedor fue eliminado", "success");
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

