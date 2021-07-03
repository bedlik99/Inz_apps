package com.example.localserver.Repositories;

import com.example.localserver.Entity.RecordedEvent;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

@Repository
public interface RecordedEventRepository extends JpaRepository<RecordedEvent, Long> {
}
