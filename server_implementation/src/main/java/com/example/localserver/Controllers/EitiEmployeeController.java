package com.example.localserver.Controllers;

import com.example.localserver.DTO.RecordedEventDTO;
import com.example.localserver.UserService.EitiLabUserService;
import com.example.localserver.UserService.EmployeeUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.authentication.AnonymousAuthenticationToken;
import org.springframework.security.core.Authentication;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.RequestParam;

import java.util.ArrayList;
import java.util.List;

@Controller
public class EitiEmployeeController {

    private EitiLabUserService eitiLabUserService;
    private EmployeeUserService employeeUserService;

    public EitiEmployeeController(EitiLabUserService eitiLabUserService,
                                  EmployeeUserService employeeUserService) {
        this.eitiLabUserService = eitiLabUserService;
        this.employeeUserService = employeeUserService;
    }

    @GetMapping("/")
    public String showLeadPage(){
        return "redirect:/login";
    }

    @GetMapping("/login")
    public String showLoginForm(){
        if(isAuthenticated()){
            return "redirect:/logged/checkLogs";
        }
        return "login-form";
    }

    @GetMapping("/logged/checkLogs")
    public String showTeachersPage(Model theModel){
        theModel.addAttribute("recordedLogs", new ArrayList<>());
        theModel.addAttribute("chosenIdx","");
        return "teacher-page";
    }

    @PostMapping("/logged/findStudentLogs")
    public String showStudentsRecords(@RequestParam(name="submitButtonIndex") String indexNr,
                                      Model theModel){
        List<RecordedEventDTO> recordedEvents = eitiLabUserService.findStudentRegistries(indexNr);
        theModel.addAttribute("recordedLogs",recordedEvents);
        theModel.addAttribute("chosenIdx",indexNr);
        return "teacher-page";
    }

    private boolean isAuthenticated() {
        Authentication authentication = SecurityContextHolder.getContext().getAuthentication();
        if (authentication == null || AnonymousAuthenticationToken.class.isAssignableFrom(authentication.getClass())) {
            return false;
        }
        return authentication.isAuthenticated();
    }

}
