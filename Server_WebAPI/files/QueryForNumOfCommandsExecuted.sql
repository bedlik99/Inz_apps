SELECT RegisteredUserItems.Id, Count(RecordedEventItems.RegistryContent) as LiczbaKomend
FROM RegisteredUserItems
INNER JOIN RecordedEventItems ON RegisteredUserItems.Id = RecordedEventItems.RegisteredUserId
GROUP BY RegisteredUserItems.Id

HAVING Count(RecordedEventItems.RegistryContent) > 1
ORDER BY LiczbaKomend