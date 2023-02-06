// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function BuscaCepDestinatario()
{
    $(document).ready(function () {

        function limpa_formulário_cep() {
            // Limpa valores do formulário de cep.
            $("#EnderecosDestinatario_Logradouro").val("");
            $("#EnderecosDestinatario_Bairro").val("");
            $("#EnderecosDestinatario_Cidade").val("");
            $("#EnderecosDestinatario_Estado").val("");
        }

        //Quando o campo cep perde o foco.
        $("#EnderecosDestinatario_CEP").blur(function () {

            //Nova variável "cep" somente com dígitos.
            var cep = $(this).val().replace(/\D/g, '');

            //Verifica se campo cep possui valor informado.
            if (cep != "") {

                //Expressão regular para validar o CEP.
                var validacep = /^[0-9]{8}$/;

                //Valida o formato do CEP.
                if (validacep.test(cep)) {

                    //Preenche os campos com "..." enquanto consulta webservice.
                    $("#EnderecosDestinatario_Logradouro").val("...");
                    $("#EnderecosDestinatario_Bairro").val("...");
                    $("#EnderecosDestinatario_Cidade").val("...");
                    $("#EnderecosDestinatario_Estado").val("...");

                    //Consulta o webservice viacep.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valores da consulta.
                                $("#EnderecosDestinatario_Logradouro").val(dados.logradouro);
                                $("#EnderecosDestinatario_Bairro").val(dados.bairro);
                                $("#EnderecosDestinatario_Cidade").val(dados.localidade);
                                $("#EnderecosDestinatario_Estado").val(dados.uf);
                            } //end if.
                            else {
                                //CEP pesquisado não foi encontrado.
                                limpa_formulário_cep();
                                alert("CEP não encontrado.");
                            }
                        });
                } //end if.
                else {
                    //cep é inválido.
                    limpa_formulário_cep();
                    alert("Formato de CEP inválido.");
                }
            } //end if.
            else {
                //cep sem valor, limpa formulário.
                limpa_formulário_cep();
            }
        });
    });
}

function BuscaCepTransportadores() {
    $(document).ready(function () {

        function limpa_formulário_cep() {
            // Limpa valores do formulário de cep.
            $("#EnderecosTransportador_Logradouro").val("");
            $("#EnderecosTransportador_Bairro").val("");
            $("#EnderecosTransportador_Cidade").val("");
            $("#EnderecosTransportador_Estado").val("");
        }

        //Quando o campo cep perde o foco.
        $("#EnderecosTransportador_CEP").blur(function () {

            //Nova variável "cep" somente com dígitos.
            var cep = $(this).val().replace(/\D/g, '');

            //Verifica se campo cep possui valor informado.
            if (cep != "") {

                //Expressão regular para validar o CEP.
                var validacep = /^[0-9]{8}$/;

                //Valida o formato do CEP.
                if (validacep.test(cep)) {

                    //Preenche os campos com "..." enquanto consulta webservice.
                    $("#EnderecosTransportador_Logradouro").val("...");
                    $("#EnderecosTransportador_Bairro").val("...");
                    $("#EnderecosTransportador_Cidade").val("...");
                    $("#EnderecosTransportador_Estado").val("...");

                    //Consulta o webservice viacep.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valores da consulta.
                                $("#EnderecosTransportador_Logradouro").val(dados.logradouro);
                                $("#EnderecosTransportador_Bairro").val(dados.bairro);
                                $("#EnderecosTransportador_Cidade").val(dados.localidade);
                                $("#EnderecosTransportador_Estado").val(dados.uf);
                            } //end if.
                            else {
                                //CEP pesquisado não foi encontrado.
                                limpa_formulário_cep();
                                alert("CEP não encontrado.");
                            }
                        });
                } //end if.
                else {
                    //cep é inválido.
                    limpa_formulário_cep();
                    alert("Formato de CEP inválido.");
                }
            } //end if.
            else {
                //cep sem valor, limpa formulário.
                limpa_formulário_cep();
            }
        });
    });
}


function BuscaCepGeradores() {
    $(document).ready(function () {

        function limpa_formulário_cep() {
            // Limpa valores do formulário de cep.
            $("#EnderecosGerador_Logradouro").val("");
            $("#EnderecosGerador_Bairro").val("");
            $("#EnderecosGerador_Cidade").val("");
            $("#EnderecosGerador_Estado").val("");
        }

        //Quando o campo cep perde o foco.
        $("#EnderecosGerador_CEP").blur(function () {

            //Nova variável "cep" somente com dígitos.
            var cep = $(this).val().replace(/\D/g, '');

            //Verifica se campo cep possui valor informado.
            if (cep != "") {

                //Expressão regular para validar o CEP.
                var validacep = /^[0-9]{8}$/;

                //Valida o formato do CEP.
                if (validacep.test(cep)) {

                    //Preenche os campos com "..." enquanto consulta webservice.
                    $("#EnderecosGerador_Logradouro").val("...");
                    $("#EnderecosGerador_Bairro").val("...");
                    $("#EnderecosGerador_Cidade").val("...");
                    $("#EnderecosGerador_Estado").val("...");

                    //Consulta o webservice viacep.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valores da consulta.
                                $("#EnderecosGerador_Logradouro").val(dados.logradouro);
                                $("#EnderecosGerador_Bairro").val(dados.bairro);
                                $("#EnderecosGerador_Cidade").val(dados.localidade);
                                $("#EnderecosGerador_Estado").val(dados.uf);
                            } //end if.
                            else {
                                //CEP pesquisado não foi encontrado.
                                limpa_formulário_cep();
                                alert("CEP não encontrado.");
                            }
                        });
                } //end if.
                else {
                    //cep é inválido.
                    limpa_formulário_cep();
                    alert("Formato de CEP inválido.");
                }
            } //end if.
            else {
                //cep sem valor, limpa formulário.
                limpa_formulário_cep();
            }
        });
    });
}



function BuscaCepCtrs() {
    $(document).ready(function () {

        function limpa_formulário_cep() {
            // Limpa valores do formulário de cep.
            $("#logradouro").val("");
            $("#bairro").val("");
            $("#ciadde").val("");
            $("#numero").val("");
            //$("#EnderecosTransportador_Estado").val("");
        }

        //Quando o campo cep perde o foco.
        $("#cepx").blur(function () {

            //Nova variável "cep" somente com dígitos.
            var cep = $(this).val().replace(/\D/g, '');

            //Verifica se campo cep possui valor informado.
            if (cep != "") {

                //Expressão regular para validar o CEP.
                var validacep = /^[0-9]{8}$/;

                //Valida o formato do CEP.
                if (validacep.test(cep)) {

                    //Preenche os campos com "..." enquanto consulta webservice.
                    $("#logradouro").val("...");
                    $("#bairro").val("...");
                    $("#ciadde").val("...");
                    //$("#EnderecosTransportador_Estado").val("...");

                    //Consulta o webservice viacep.com.br/
                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {

                            if (!("erro" in dados)) {
                                //Atualiza os campos com os valores da consulta.
                                $("#logradouro").val(dados.logradouro);
                                $("#bairro").val(dados.bairro);
                                $("#ciadde").val(dados.localidade);
                                $("#numero").val("");
                            
                                //$("#EnderecosTransportador_Estado").val(dados.uf);
                            } //end if.
                            else {
                                //CEP pesquisado não foi encontrado.
                                limpa_formulário_cep();
                                alert("CEP não encontrado.");
                            }
                        });
                } //end if.
                else {
                    //cep é inválido.
                    limpa_formulário_cep();
                    alert("Formato de CEP inválido.");
                }
            } //end if.
            else {
                //cep sem valor, limpa formulário.
                limpa_formulário_cep();
            }
        });
    });
}