package com.example.localserver.DTO;

public class RecordedEventDTO {

    private String mail;
    private String registryContent;

    public RecordedEventDTO(){}

    public RecordedEventDTO(String mail, String registryContent) {
        this.mail = mail;
        this.registryContent = registryContent;
    }

    public String getMail() {
        return mail;
    }

    public void setMail(String mail) {
        this.mail = mail;
    }

    public String getRegistryContent() {
        return registryContent;
    }

    public void setRegistryContent(String registryContent) {
        this.registryContent = registryContent;
    }

    @Override
    public String toString() {
        return "{" + "mail='" + mail + '\'' +
                ", registryContent='" + registryContent + '\'' +
                '}';
    }
}
