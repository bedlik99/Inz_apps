package com.example.localserver.exceptions;


public class CustomMessageNotAcceptableException extends RuntimeException {

    private final String message;

    public CustomMessageNotAcceptableException(String message) {
        this.message = message;
    }

    @Override
    public String toString() {
        return message;
    }
}
