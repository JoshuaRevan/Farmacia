const MODELO_BASE = {
    idUsuario: 0,
    nombre: "",
    apellido: "",
    telefono: "",
    correo: "",
    IdCargo: 0
};

let tablaData;

$(function () {

    // Reinicia DataTable si ya existe
    if ($.fn.DataTable.isDataTable('#tbdata')) {
        $('#tbdata').DataTable().destroy();
    }

    // Cargar los cargos en el combo
    fetch("/Usuario/ListaCargo")
        .then(response => response.json()) 
        .then(data => {
            $("#cboCargo").empty();
            $("#cboCargo").append('<option value="">Seleccione un cargo</option>');

            data.data.forEach(item => {
                $("#cboCargo").append(
                    $("<option>").val(item.idCargo).text(item.cargo)  //Guarda el idCargo en la BD no el texto
                );
            });
        })
        .catch(error => console.error("Error al cargar cargos:", error));

    // Inicializar DataTable
    tablaData = $('#tbdata').DataTable({
        responsive: true,
        ajax: {
            url: '/Usuario/Lista',
            type: 'GET',
            datatype: 'json',
            dataSrc: 'data'
        },
        columns: [
            { data: "idUsuario", visible: false, searchable: false },
            { data: "nombreUsuario", title: "Nombre" },
            { data: "apellidoUsuario", title: "Apellido" },
            { data: "telefono", title: "Teléfono" },
            { data: "correo", title: "Correo" },
            { data: "idCargo", title: "ID Cargo" },
            { data: "nombreCargo", title: "Cargo" },
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
                filename: 'Reporte Usuarios',
                exportOptions: {
                    columns: [1, 2, 3, 4, 5, 6] // Columnas visibles
                }
            },
            'pageLength'
        ],
        language: {
            url: "https://cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
        }
    });
});

// Mostrar el modal con datos o con campos vacíos
function mostrarModal(modelo = MODELO_BASE) {
    $("#txtId").val(modelo.idUsuario);
    $("#txtNombre").val(modelo.nombreUsuario);
    $("#txtApellido").val(modelo.apellidoUsuario);
    $("#txtTelefono").val(modelo.telefono);
    $("#txtCorreo").val(modelo.correo);
    $("#cboCargo").val(
        modelo.idCargo == 0 ? $("#cboCargo option:first").val() : modelo.idCargo);

    $("#modalData").modal("show");
}

// Botón de modal para crear nuevo usuario
$("#btnNuevoUsuario").click(function () {
    mostrarModal();
});

$("#btnGuardar").click(function () {

    /*debugger;*/

    const inputs = $("input.input-validar").serializeArray();
    const inputs_sin_valor = inputs.filter((item) => item.value.trim() == "")

    if (inputs_sin_valor.length > 0) {
        const mensaje = `Debe de completar el campo :"${inputs_sin_valor[0].name}"`;
        toastr.warning("", mensaje)
        $(`input[name="${inputs_sin_valor[0].name}"]`).focus()
        return;
    }
    const modelo = structuredClone(MODELO_BASE);
    modelo["idUsuario"] = parseInt($("#txtId").val())
    modelo["nombreUsuario"] = $("#txtNombre").val()
    modelo["apellidoUsuario"] = $("#txtApellido").val()
    modelo["telefono"] = $("#txtTelefono").val()
    modelo["correo"] = $("#txtCorreo").val()
    modelo["IdCargo"] = $("#cboCargo").val()

    $("#modalData").find("div.modal-content").LoadingOverlay("show");

    if (modelo.idUsuario == 0) {
        fetch("/Usuario/Crear", {
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

                    const passwordGenerada = responseJson.objeto.contrasena || "No disponible";
                    swal({
                        title: "Usuario creado exitosamente",
                        text: `La contraseña generada es:\n\n${passwordGenerada}\n\nCópiala antes de continuar.`,
                        icon: "success",
                        button: "entendido"
                    });

                } else {
                    swal("Lo sentimos!", responseJson.mensaje, "error");
                }
            })
            .catch(error => {
                $("#modalData").find("div.modal-content").LoadingOverlay("hide");
                console.error("Error al crear usuario:", error);
                swal("Error", "No se pudo crear el usuario", "error");
            });

    } else {
        fetch("/Usuario/Editar", {
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
                    swal("Listo!", "El usuario fue modificado correctamente", "success")
                } else {
                    swal("Lo sentimos", responseJson.mensaje, "error")
                }
            })
    }
});

let filaSeleccionada;
$("#tbdata tbody").on("click",".btn-editar",function(){
    if ($(this).closest("tr").hasClass("child")){
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
        text: `Eliminar usuario "${data.nombreUsuario}"`,
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
                url: `/Usuario/Eliminar?idUsuario=${data.idUsuario}`,
                type: "DELETE",
                dataType: "json",
                success: function (responseJson) {
                    if (responseJson.estado) {
                        tablaData.row(fila).remove().draw();
                        swal("¡Listo!", "El usuario fue eliminado", "success");
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