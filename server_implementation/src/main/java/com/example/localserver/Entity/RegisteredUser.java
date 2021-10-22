package com.example.localserver.Entity;

import javax.persistence.*;
import java.util.Set;

@Entity
@Table(name = "registered_user")
public class RegisteredUser {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long id;
    private String email;
    private String uniqueCode;

    @OneToMany(mappedBy = "registeredUser")
    private Set<RecordedEvent> eventRegistries;

    public RegisteredUser() {}

    public RegisteredUser(String email, String uniqueCode) {
        this.email = email;
        this.uniqueCode = uniqueCode;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getEmail(){
        return email;
    }

    public void setEmail(String email){
        this.email = email;
    }

    public String getUniqueCode() {
        return uniqueCode;
    }

    public void setUniqueCode(String uniqueCode) {
        this.uniqueCode = uniqueCode;
    }

    public Set<RecordedEvent> getEventRegistries() {
        return eventRegistries;
    }

    public void setEventRegistries(Set<RecordedEvent> eventRegistries) {
        this.eventRegistries = eventRegistries;
    }
}
