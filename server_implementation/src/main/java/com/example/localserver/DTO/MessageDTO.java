package com.example.localserver.DTO;

public class MessageDTO {

    private String value;

    public MessageDTO(){}

    public MessageDTO(String value) {
        this.value = value;
    }

    public String getValue() {
        return value;
    }

    public void setValue(String value) {
        this.value = value;
    }

    @Override
    public String toString() {
        return "{" + "value='" + value + '\'' + '}';
    }
}
