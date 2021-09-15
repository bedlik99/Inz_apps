package com.example.localserver.UserService;

import com.example.localserver.Entity.EmployeeUser;
import com.example.localserver.Entity.Role;
import com.example.localserver.Repositories.EmployeeUserRepository;
import org.springframework.security.core.GrantedAuthority;
import org.springframework.security.core.authority.SimpleGrantedAuthority;
import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import java.util.Collection;
import java.util.stream.Collectors;

@Service
public class EmployeeUserService implements UserDetailsService {

    private EmployeeUserRepository employeeUserRepository;

    public EmployeeUserService(EmployeeUserRepository employeeUserRepository) {
        this.employeeUserRepository = employeeUserRepository;
    }

    @Override
    @Transactional
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        EmployeeUser user = employeeUserRepository.findEmployeeUserByUsername(username);
        if (user == null) {
            throw new UsernameNotFoundException("Could not find user");
        }
        return new org.springframework.security.core.userdetails.User(user.getUsername(), user.getPassword(),
                mapRolesToAuthorities(user.getRoles()));
    }

    private Collection<? extends GrantedAuthority> mapRolesToAuthorities(Collection<Role> roles) {
        return roles.stream().map(role -> new SimpleGrantedAuthority(role.getRoleName())).collect(Collectors.toList());
    }

    @Transactional
    public EmployeeUser getLoggedUser() {
        Object principal = SecurityContextHolder.getContext().getAuthentication().getPrincipal();
        String username;
        if (principal instanceof UserDetails) {
            username = ((UserDetails) principal).getUsername();
        } else {
            username = principal.toString();
        }
        return employeeUserRepository.findEmployeeUserByUsername(username);
    }
}
