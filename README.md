# **PDI_21L**
The aim of this work is to implement a remote system to assess the self-efficacy of a laboratory exercise.
## Engineering diploma thesis performed by
* Jan Bedliński
* Kamil Chrościcki
## The supervisor of the thesis is
* Mateusz Żotkiewicz, Ph.D. D.Sc.

<br>
<hr>
<h2>Running Client</h2>
<p><b>Install cmake on your machine.</b></p>
<p>Install CPPRESTSDK libraries on your machine using tutorial from the main repo: https://github.com/microsoft/cpprestsdk </p>

<p>Move into client directory.</p>
    1. Delete CMakeCache.txt file. <br>
    2. Run command

```
cmake .
```
To rebuild needed files.

3. To run client execute bash script with command.<p>

```
./startClient
```
<hr>
<p>Bash script is calling doing 3 things. The commands used in bash can be seen when opening 'startClient' script. The commands are:</p>

```
1) echo "" > results.txt
2) cmake --build .
3) ./HttpRestClient
```

 1) Clear results.txt file when launching client.
 2) Build project.
 3) Run HttpRestClient.o file. This file is being build everytime project is build(2.).

<hr>
<h2>Running server (Temporary)</h2>
<p>Server was written using Java version 11 so it is recommended version to have on your local machine. However 1.8 Java version should be also compatible.<p>
<p>Only Jar file is added. To run server simply execute jar file with command:</p>

```
java -jar localserver_example.jar
```
<p>Server runs on URL: http://localhost:8080<p><hr>
<h3><b>To see the content of H2 database:</b></h3>

1. Go to database URL: http://localhost:8080/h2-console
    
2. Fill the box like below:

    * 'Saved settings' and 'Setting name': Generic H2 (Embedded)
    * 'Driver class': org.h2.Driver
    * JDBC URL: jdbc:h2:file:./data/database
    * Credentials:
        * User name: sa
        * Password: password

3. Click connect(enter) and in the left corner choose 'STUDENT' label. Now in the middle bar the execute statement should be filled. Finally click 'run' in the bar above.

