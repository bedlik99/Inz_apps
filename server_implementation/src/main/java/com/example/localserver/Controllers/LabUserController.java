package com.example.localserver.Controllers;

import com.example.localserver.DTO.MessageDTO;
import com.example.localserver.UserService.EitiLabUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseStatus;

@Controller
public class LabUserController {

    private EitiLabUserService eitiLabUserService;

    @Autowired
    public LabUserController(EitiLabUserService eitiLabUserService) {
        this.eitiLabUserService = eitiLabUserService;
    }

    @PostMapping(consumes = "application/json",value = "/registerUser")
    public ResponseEntity<String> registerUser(@RequestBody MessageDTO encryptedMessage) {
        return eitiLabUserService.processUserInitData(encryptedMessage);
    }

    @PostMapping(consumes = "application/json",value = "/recordEvent")
    @ResponseStatus(value = HttpStatus.OK)
    public void recordEvent(@RequestBody MessageDTO encryptedMessage){
        eitiLabUserService.processEventContent(encryptedMessage);
    }
}