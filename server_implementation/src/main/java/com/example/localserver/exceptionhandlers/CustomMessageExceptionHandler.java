package com.example.localserver.exceptionhandlers;

import com.example.localserver.exceptions.CustomMessageNotAcceptableException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.ControllerAdvice;
import org.springframework.web.bind.annotation.ExceptionHandler;

@ControllerAdvice
public class CustomMessageExceptionHandler {

    @ExceptionHandler(value = CustomMessageNotAcceptableException.class)
    public ResponseEntity<Object> handleCustomMessageNotAcceptableException(CustomMessageNotAcceptableException exception) {
        return new ResponseEntity<>(exception.getMessage(), HttpStatus.NOT_ACCEPTABLE);
    }

}
