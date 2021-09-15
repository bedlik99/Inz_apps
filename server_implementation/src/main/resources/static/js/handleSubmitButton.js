window.onload = () => {
    const numRegex = new RegExp('[0-9]');
    let submitForm = document.getElementById("submitFormId");
    let indexInput = document.getElementById('inputId');
    let submitButton = document.getElementById('submitButtonId');

    indexInput.addEventListener('input',() => {
        if(numRegex.test(indexInput.value.substring(indexInput.value.length-1))){
            if(indexInput.value.length > 6){
                indexInput.value = indexInput.value.substring(0,6);
            }
        }else{
            indexInput.value = indexInput.value.substring(0,indexInput.value.length-1);
        }
    });

    submitButton.addEventListener('click',() => {
        if(indexInput.value.length != 6){
            submitForm.onsubmit = () => {
                return false;
            }
            alert("Podano nieprawidlowy numer indeksu");
        }else{
            submitForm.onsubmit = () => {
                submitButton.value = indexInput.value;
                return true;
            }
        }
    });
};