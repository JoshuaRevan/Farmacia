const MODELO_BASE = {
    idProducto: 0,
    nombreProducto: "",
    descripcion: "",
    ubicacion: "",
    idMarca: 0,
    nombreMarca: "",
    idPresentacion: 0,
    nombrePresentacion: "",
    cantidad: 0,
    precio: 0,
    fechaVencimiento: "",
    loteInterno: "",
}

let tablaData;

$(function () {
    // Destruir tabla existente si hay una
    if ($.fn.DataTable.isDataTable('#tbdata')) {
        $('#tbdata').DataTable().destroy();
    }

    // Inicializar DataTable 
    tablaData = $('#tbdata').DataTable({
        responsive: true,
        ajax: {
            url: '/Producto/Lista',
            type: 'GET',
            datatype: 'json',
            dataSrc: 'data' // ← porque retornas { data: [...] }
        },
        columns: [
            { data: "idProducto", visible: false, searchable: false },
            { data: "nombreProducto", title: "Nombre Producto" },
            { data: "descripcion", title: "Descripción" },
            { data: "ubicacion", title: "Ubicación" },
            { data: "idMarca", title: "IdMarca", visible: false, searchable: false },
            { data: "nombreMarca", title: "Marca" },
            { data: "idPresentacion", title: "Id Presentacion", visible: false, searchable: false },
            { data: "nombrePresentacion", title: "Presentación" },
            {
                data: "cantidad",
                title: "Cantidad",
                defaultContent: "0",
                render: function (data, type, row) {
                    return data !== null && data !== undefined ? data : 0;
                }
            },
            {
                data: "precio",
                title: "Precio",
                defaultContent: "0",
                render: function (data, type, row) {
                    return data !== null && data !== undefined ? `$${parseFloat(data).toFixed(2)}` : "$0.00";
                }
            },
            {
                data: "fechaVencimiento",
                title: "Fecha Vencimiento",
                defaultContent: "",
                render: function (data) {
                    if (!data) return "";
                    const fecha = new Date(data);
                    return fecha.toLocaleDateString("es-ES");
                }
            },
            {
                data: "loteInterno",
                title: "Lote Interno",
                defaultContent: "",
                render: function (data) {
                    return data !== null && data !== undefined ? data : "";
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    let botones = "";

                    if (row.puedeEditar) {
                        botones += `
                            <button class="btn btn-primary btn-editar btn-sm mr-2">
                                <i class="fas fa-pencil-alt"></i>
                            </button>`;
                    }
                    if (row.puedeEliminar) {
                        botones += `
                            <button class="btn btn-danger btn-eliminar btn-sm">
                                <i class="fas fa-trash-alt"></i>
                            </button>`;
                    }
                    return botones; 
                },
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
                title: 'Reporte Productos',
                filename: 'Reporte_Productos',
                exportOptions: {
                    columns: [1, 2, 3, 5, 7, 8, 9, 10, 11] // Columnas visibles
                }
            },
            'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        },
    });
});

// CORRECCIÓN: Hacer que cargarCombobox retorne una promesa
function cargarCombobox(modelo = MODELO_BASE) {
    let marcasPromise = fetch('/Marcas/Lista').then(r => r.json());
    let presentacionesPromise = fetch('/Presentaciones/Lista').then(r => r.json());

    // RETORNAR la promesa
    return Promise.all([marcasPromise, presentacionesPromise]).then(([marcas, presentaciones]) => {
        // Llenar marcas
        $("#cmbMarca").empty().append('<option value="">Seleccione una marca</option>');
        marcas.data.forEach(item => {
            $("#cmbMarca").append($("<option>").val(item.idMarca).text(item.nombreMarca));
        });
        $("#cmbMarca").val(modelo.idMarca == 0 ? "" : modelo.idMarca);

        // Llenar presentaciones
        $("#cmbPresentacion").empty().append('<option value="">Seleccione una presentación</option>');
        presentaciones.data.forEach(item => {
            $("#cmbPresentacion").append($("<option>").val(item.idPresentacion).text(item.tipoPresentacion));
        });
        $("#cmbPresentacion").val(modelo.idPresentacion == 0 ? "" : modelo.idPresentacion);
    });
}

function mostrarModal(modelo = MODELO_BASE) {
    cargarCombobox(modelo).then(() => {
        $("#txtId").val(modelo.idProducto);
        $("#txtNombreProducto").val(modelo.nombreProducto);
        $("#txtUbicacion").val(modelo.ubicacion);
        $("#txtDescripcionProducto").val(modelo.descripcion);

        $("#modalData").modal("show");
    });
}

$("#btnNuevo").on('click', function () {
    console.log('Botón Nuevo clickeado');
    try {
        mostrarModal();
    } catch (error) {
        console.error('Error en btnNuevo click:', error);
    }
});

$("#btnAgregarMarca").on("click", function () {
    // Redirigir a la vista de creación de marca
    window.location.href = "/Marcas/";
});

$("#btnAgregarPresentacion").on("click", function () {
    // Redirigir a la vista de creación de marca
    window.location.href = "/Presentaciones/";
});

$("#btnGuardar").on("click", function () {
    if ($("#txtNombreProducto").val().trim() == "") {
        toastr.warning("", "debe de complementar el campo")
        $("#txtNombreProducto").focus()
        return;
    }
    if ($("#txtUbicacion").val().trim() == "") {
        toastr.warning("", "debe de completar el campo")
        $("#txtUbicacion").focus()
        return;
    }
    if ($("#txtDescripcionProducto").val().trim() == "") {
        toastr.warning("", "debe de complementar el campo")
        $("#txtDescripcionProducto").focus()
        return;
    }
    if ($("#cmbMarca").val().trim() == "") {
        toastr.warning("", "debe de complementar el campo")
        $("#cmbMarca").focus()
        return;
    }
    if ($("#cmbPresentacion").val().trim() == "") {
        toastr.warning("", "debe de complementar el campo")
        $("#cmbPresentacion").focus()
        return;
    }

    const modelo = structuredClone(MODELO_BASE);
    modelo["idProducto"] = parseInt($("#txtId").val())
    modelo["nombreProducto"] = $("#txtNombreProducto").val()
    modelo["ubicacion"] = $("#txtUbicacion").val()
    modelo["descripcion"] = $("#txtDescripcionProducto").val()
    modelo["idMarca"] = parseInt($("#cmbMarca").val())
    modelo["idPresentacion"] = parseInt($("#cmbPresentacion").val())

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.idProducto == 0) {
        fetch("/Producto/Crear", {
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
                        title: "Producto creado exitosamente",
                        icon: "success", // CORRECCIÓN: "success" en lugar de "Succes"
                        button: "entendido"
                    });
                } else {
                    swal("Lo sentimos!", responseJson.mensaje, "error");
                }
            })
            .catch(error => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                console.error("error al crear Producto: ", error);
                swal("Error", "No se puede crear el Producto", "error");
            });
    } else {
        fetch("/Producto/Editar", {
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
                    swal("Listo!", "EL Producto fue modificado correctamente", "success") // CORRECCIÓN: "success" en lugar de "Success"
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
            .catch(error => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                console.error("error al editar Producto: ", error);
                swal("Error", "No se puede editar el Producto", "error");
            });
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
        text: `Eliminar Producto "${data.nombreProducto}"`, // CORRECCIÓN: usar nombreProducto
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
                url: `/Producto/Eliminar?idProducto=${data.idProducto}`,
                type: "DELETE",
                dataType: "json",
                success: function (responseJson) { // CORRECCIÓN: "success" en lugar de "succes"
                    if (responseJson.estado) {
                        tablaData.row(fila).remove().draw(); // CORRECCIÓN: Eliminar la fila de la tabla
                        swal("¡Listo!", "El Producto fue eliminado", "success");
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