package com.example.localserver.Repositories;

import com.example.localserver.Entity.EmployeeUser;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface EmployeeUserRepository extends JpaRepository<EmployeeUser,Long> {
    EmployeeUser findEmployeeUserByUsername(String username);
}
