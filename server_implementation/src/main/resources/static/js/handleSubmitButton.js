window.onload = () => {
    const numRegex = new RegExp('[0-9]');
    let submitForm = document.getElementById("submitFormId");
    let indexInput = document.getElementById('inputId');
    let submitButton = document.getElementById('submitButtonId');

    indexInput.addEventListener('input',() => {
        if(numRegex.test(indexInput.value.substring(indexInput.value.length-1))){
            if(indexInput.value.length > 8){
                indexInput.value = indexInput.value.substring(0,8);
            }
        }else{
            indexInput.value = indexInput.value.substring(0,indexInput.value.length-1);
        }
    });

    submitButton.addEventListener('click',() => {
        if(indexInput.value.length != 8){
            submitForm.onsubmit = () => {
                return false;
            }
            alert("Podano nieprawidlowy numer email");
        }else{
            submitForm.onsubmit = () => {
                submitButton.value = indexInput.value;
                return true;
            }
        }
    });
};