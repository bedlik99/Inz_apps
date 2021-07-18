package com.example.localserver.UserService;

import com.example.localserver.DTO.MessageDTO;
import com.example.localserver.DTO.RecordedEventDTO;
import com.example.localserver.DTO.RegisteredUserDTO;
import com.example.localserver.Entity.RecordedEvent;
import com.example.localserver.Entity.RegisteredUser;
import com.example.localserver.Repositories.EitiUserRepository;
import com.example.localserver.Repositories.RecordedEventRepository;
import com.example.localserver.Util.CryptoUtil;
import com.google.gson.Gson;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.stereotype.Service;

import javax.crypto.BadPaddingException;
import javax.crypto.IllegalBlockSizeException;
import javax.crypto.NoSuchPaddingException;
import java.security.InvalidAlgorithmParameterException;
import java.security.InvalidKeyException;
import java.security.NoSuchAlgorithmException;
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

    public ResponseEntity<String> processUserInitData(MessageDTO encryptedMessage){
        RegisteredUserDTO registeredUser = decryptMessage(encryptedMessage.getValue());
        if(registeredUser!=null){
            if(validateUserData(registeredUser)){
                eitiUserRepository.save(new RegisteredUser(registeredUser.getIndexNr(),registeredUser.getUniqueCode()));
                return ResponseEntity.status(HttpStatus.OK).body(encryptMessage(registeredUser.getIndexNr()));
            }
            return ResponseEntity.status(HttpStatus.NOT_ACCEPTABLE).body("");
        }
        return ResponseEntity.status(HttpStatus.NOT_ACCEPTABLE).body("");
    }

    private String encryptMessage(String indexNr) {
        String encryptedResponse="";
        try{
            encryptedResponse = CryptoUtil.encrypt(indexNr);
        }catch (InvalidAlgorithmParameterException | NoSuchPaddingException | IllegalBlockSizeException
                | NoSuchAlgorithmException | BadPaddingException | InvalidKeyException e) {
            e.printStackTrace();
        }
        return encryptedResponse;
    }

    private RegisteredUserDTO decryptMessage(String encryptedMessage)  {
        RegisteredUserDTO registeredUserDTO=null;
        try{
            String decryptedMessage = CryptoUtil.decrypt(encryptedMessage);
            registeredUserDTO = new Gson().fromJson(decryptedMessage,RegisteredUserDTO.class);
        }catch (InvalidAlgorithmParameterException | NoSuchPaddingException | IllegalBlockSizeException
                | NoSuchAlgorithmException | BadPaddingException | InvalidKeyException e) {
            e.printStackTrace();
        }
        return registeredUserDTO;
    }

    public void processEventContent(RecordedEventDTO recordedEventDTO) {
        RegisteredUser searchedUser = eitiUserRepository.findStudentByIndexNum(recordedEventDTO.getIndexNr());
        if(searchedUser != null){
            recordedEventRepository.save(new RecordedEvent(recordedEventDTO.getRegistryContent(), LocalDateTime.now(),searchedUser));
        }
    }

    private boolean validateUserData(RegisteredUserDTO registeredUserDTO){
        return registeredUserDTO.getIndexNr().matches("[0-9]{6}") &&
                registeredUserDTO.getUniqueCode().length() == 6;
    }
}
