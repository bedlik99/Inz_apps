package com.example.localserver.UserService;

import com.example.localserver.DTO.RecordedEventDTO;
import com.example.localserver.DTO.RegisteredUserDTO;
import com.example.localserver.Entity.RecordedEvent;
import com.example.localserver.Entity.RegisteredUser;
import com.example.localserver.Repositories.EitiUserRepository;
import com.example.localserver.Repositories.RecordedEventRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import java.time.LocalDateTime;

@Service
public class EitiUserService {

    private final EitiUserRepository eitiUserRepository;
    private final RecordedEventRepository recordedEventRepository;

    @Autowired
    public EitiUserService(EitiUserRepository eitiUserRepository,RecordedEventRepository recordedEventRepository) {
        this.eitiUserRepository = eitiUserRepository;
        this.recordedEventRepository = recordedEventRepository;
    }

    public ResponseEntity<String> processUserInitData(RegisteredUserDTO registeredUserDTO){
        if(validateUserData(registeredUserDTO)){
            eitiUserRepository.save(new RegisteredUser(registeredUserDTO.getIndexNum(),registeredUserDTO.getUniqueCode()));
            return ResponseEntity.status(HttpStatus.OK).body("Accepted");
        }
        return ResponseEntity.status(HttpStatus.NOT_ACCEPTABLE).body("Verify your credentials and send again!");
    }

    public void processEventContent(RecordedEventDTO recordedEventDTO) {
        RegisteredUser searchedUser = eitiUserRepository.findStudentByIndexNum(recordedEventDTO.getIndexNum());
        if(searchedUser != null){
            recordedEventRepository.save(new RecordedEvent(recordedEventDTO.getRegistryContent(), LocalDateTime.now(),searchedUser));
        }
    }

    private boolean validateUserData(RegisteredUserDTO registeredUserDTO){
        return registeredUserDTO.getIndexNum().matches("[0-9]{6}") &&
                registeredUserDTO.getUniqueCode().length() >= 6;
    }
}
