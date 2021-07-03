package com.example.localserver.Repositories;

import com.example.localserver.Entity.RegisteredUser;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface EitiUserRepository extends JpaRepository<RegisteredUser, Long> {
        RegisteredUser findStudentByIndexNum(String indexNum);
}
