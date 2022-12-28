function AjaxModal() {
    $(document).ready(function () {
        $(function () {
            $.ajaxSetup({ cache: false });

            $("a[data-modal]").on("click",
                function (e) {
                    $('#myModalContent').load(this.href,
                        function () {
                            $('#myModal').modal({
                                keyboard: true,
                            },
                                'show');
                            bindForm(this);
                        });
                    return false;
                });
        });

        function bindForm(dialog) {
            $('form', dialog).submit(function () {
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            $('#myModal').modal('hide');
                            $('#AddressTarget').load(result.url); // Load the HTML result to the AddressTarget div
                        } else {
                            $('#myModalContent').html(result);
                            bindForm(dialog);
                        }
                    }
                });
                return false;
            });
        }
    });
}

function SearchZipCode() {
    $(document).ready(function () {

        function cleanAddressFormFields() {
            $("#Address_Address1").val("");
            $("#Address_Address2").val("");
            $("#Address_City").val("");
            $("#Address_Province").val("");
        }

        $("#Address_ZipCode").blur(function () {
            let zipCode = $(this).val().replace(/\D/g, '');

            if (zipCode !== "") {
                let zipCodeValidation = /^[0-9]{8}$/;

                if (zipCodeValidation.test(zipCode)) {
                    $("#Address_Address1").val("...");
                    $("#Address_City").val("...");
                    $("#Address_Province").val("...");

                    $.getJSON("https://viacep.com.br/ws/" + zipCode + "/json/?callback=?",
                        function (data) {
                            if (!("erro" in data)) {
                                $("#Address_Address1").val(data.logradouro);
                                $("#Address_Address2").val("");
                                $("#Address_City").val(data.localidade);
                                $("#Address_Province").val(data.uf);
                            } else {
                                cleanAddressFormFields();
                                alert("Zip Code not found");
                            }
                        });
                } else {
                    cleanAddressFormFields();
                    alert("Invalid zip code format.")
                }
            } else {
                // Zip code with no value
                cleanAddressFormFields();
            }
        });
    });
}

$(document).ready(function () {
    $("#messageBox").fadeOut(2500);
});