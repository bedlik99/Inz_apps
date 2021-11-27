package com.example.localserver.DTO;

public class RecordedEventDTO {

    private String email;
    private String registryContent;

    public RecordedEventDTO(){}

    public RecordedEventDTO(String email, String registryContent) {
        this.email = email;
        this.registryContent = registryContent;
    }

    public String getEmail() {
        return email;
    }

    public void setEmail(String email) {
        this.email = email;
    }

    public String getRegistryContent() {
        return registryContent;
    }

    public void setRegistryContent(String registryContent) {
        this.registryContent = registryContent;
    }

    @Override
    public String toString() {
        return "{" + "email='" + email + '\'' +
                ", registryContent='" + registryContent + '\'' +
                '}';
    }
}
