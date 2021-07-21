package com.example.localserver.Controllers;

import com.example.localserver.DTO.MessageDTO;
import com.example.localserver.UserService.EitiUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.ResponseStatus;

@Controller
public class MainController {

    public EitiUserService eitiUserService;

    @Autowired
    public MainController(EitiUserService eitiUserService) {
        this.eitiUserService = eitiUserService;
    }

    @PostMapping(consumes = "application/json",value = "/registerUser")
    public ResponseEntity<String> registerUser(@RequestBody MessageDTO encryptedMessage) {
        return eitiUserService.processUserInitData(encryptedMessage);
    }

    @PostMapping(consumes = "application/json",value = "/recordEvent")
    @ResponseStatus(value = HttpStatus.OK)
    public void recordEvent(@RequestBody MessageDTO encryptedMessage){
        eitiUserService.processEventContent(encryptedMessage);
    }

}