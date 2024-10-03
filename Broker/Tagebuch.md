# Tagesbuch Broker-Implementation
 Hier werden Stunden erfasst und gemachte Aktivitäten.
 Format noch nicht klar, aber das kann später noch umgeändert werden.

## Person: Stunden: Aktivität
  - Martin 20231006:
    - 3h: Wir sind uns mittlerweile, vermute ich, einig, dass wir Mosquitto nicht verwenden können, da das API nur mit C++ unterstütz wird. Wir machen die ganze Logik alleine. Sonst ist unser Back-Up Plan, dass wir HiveMQ verwenden, aber das ist nicht Open-Source und anscheinend wirklich nicht gut zum Weiterentwickeln geeignet. Was ich festgestellt habe, in diesen zwei Stunden, ist dass Visual Studio nur eine Möglichkeit zum Remote-Debugen hat, aber nicht zur Remote-Entwicklung. Visual Studio Code hat diese Möglichkeit und wir sollten uns mal überlegen ob wir VSCode auch verwenden können eventuell. Die Remote-Entwicklung in VSCode kann mit einem einer Extension ermöglich werden und passiert durch SSH. Man gibt Visual Studio SSH Privilegien und kann loslegen.
    Das Rennenlassen von Programmen, die in .NET Core programmiert wurden, ist angeblich ziemlich leicht. Das ist dann meine nächste Aufgabe: Entwickeln eines einfaches Programs und Laufenlassen auf dem R-Pi(dll und jsonObjekt herüberkopieren -> dll ausführen(?))
    Quellen:
    -  VSCode Remote-Entwicklung: https://code.visualstudio.com/docs/remote/ssh
    - Gegensprache zu Visual Studio: https://learn.microsoft.com/en-us/answers/questions/353503/visual-studio-2019-remote-development-via-ssh
    - https://learn.microsoft.com/en-us/visualstudio/debugger/remote-debugging-dotnet-core-linux-with-ssh?view=vs-2019
    - https://stackoverflow.com/questions/63821699/can-visual-studio-not-vs-code-do-remote-ssh-development-the-docs-says-yes-bu
    - https://learn.microsoft.com/en-us/cpp/linux/connect-to-your-remote-linux-computer?view=msvc-170&viewFallbackFrom=vs-2019
</br>
  - Martin 20231007:
    - 4h: Programmierwerkstätte. Wir haben diskutiert wie die Topics heißen sollen und haben uns für folgenden entschieden:
      1. ${Room}/${Actor oder Sensor}/${GUId}
      2. Configuration/${GUId}
      3. Status/${InfoTopic oder ErrorTopic oder WarningTopic} </br>
      Ich bin mir noch immer nicht sicher, welche Implementierung für von MQTT für C# verwenden werden: MQTTnet oder M2MQTT. Auf den ESPs wird M2MQTT verwendet, aber MQTTnet könnte eventuell besser passen.
      Eine Stunde lang habe ich mir MQTTnet und M2MQTT angeschaut und verglichen. Das werde ich weitermachen und probieren ein MQTT Broker mit C# einzurichten. Das werde ich mit Visual Studio machen, was eine Console-App sein wird. Das Exportieren zum RPi werde ich mir dann weiters anschauen; Angeblich ist es nicht so schwer, nachdem ich das publish-e.
</br>
  - Mateusz 20231007:
    - 4h: Programmierwerkstätte. Wir haben das Namensschema für die Topics festgelegt.
      1. Room/Actor oder Sensor/GUId
      2. Configuration/GUId
      3. Status/InfoTopic oder ErrorTopic oder WarningTopic </br>
    Ich habe mir MQTTnet für C# für den Broker angeschaut. Ich bin zu dem Entschluss gekommen, dass es für MQTTnet Version 4.+ keine vernünftige Dokumentation bzw. gar keine gibt. Das Einzige, auf das ich gestoßen bin, ist eine halbwegs gute Dokumentation, jedoch für eine veraltete Version von MQTTnet. https://github.com/dotnet/MQTTnet/wiki/Server
</br>
  - Martin 20231008:
    - 3h: Aufsetzen von .NET 7 auf dem RPi, sowohl auch das Schreiben einer kleinen Doku dafür. Schreiben eines Projekts unter .NET Core und das Übertragen auf den RPi. Das Projekt lässt sich starten und läuft einwandfrei auf dem RPi. Man muss die dll Datei und runtimeconfig.json Objekt nach einem Publish zum Raspberry Pi herüberkopieren, dann mit ```[dotnet file.dll]``` laufen lassen.
</br>
  - Mateusz 20231008:
    - 3h: Recherche und Testprojekt erstellt, um eine grundlegende Verbindung mit der Klassenbibliothek MQTTnet herzustellen. MQTTnet bietet zahlreiche Funktionen, wie das Senden von Nachrichten zu einem späteren Zeitpunkt oder das Herstellen einer verschlüsselten Verbindung. Nach ausführlichen Tests hat sich gezeigt, dass MQTTnet sich als unbrauchbar erwiesen hat, da der Broker beispielsweise keine Nachrichten veröffentlichen kann.
</br>
  - Martin 20231008:
    - 1h: VSCode lokal auf meinem Rechner aufgesetzt und die ferne Entwicklung auf dem RPi ausprobiert. Funktioniert eigentlich einwandfrei, aber VSCode ist nicht gut geeignet für das wofür wir eine Entwicklungsumgebung brauchen. Wir werden Visual Studio verwenden, aus jetziger Sicht.
</br>
  - Mateusz 20231009:
    - 2h: Testprojekt mit M2MQTT geschrieben. Nach einer Absprache mit Martin haben wir uns für M2MQTT anstatt für MQTTnet entschieden und die Verbindung mit Marco getestet. Allerdings wird eine Exception ausgelöst, wenn sich der ESP mit dem Broker zu verbinden versucht.
</br>
  - Martin 20231010
    - 5h: Testprojekt mehrmals bearbeitet und M2MQTT, sowie MQTTnet ausprobiert. Lokale Verbindung zwischen ASP.Net und Web App hergestellt, das meiste wurde aber von Sebastian geschrieben, aber die Verbindung steht jetzt fest und ist funktional. Verbindung zwischen ESP und Broker mehrmals versucht, aber gescheitert. Wir haben Mosquitto ausprobiert, nur um zu testen wo das Problem liegt(Broker oder ESP), aber haben es nicht hinbekommen. Wir werden jetzt .NET 6 verwendet für das Projekt, da .NET 7 nicht von ASP.Net(?) unterstütz wird. </br>
    Zusammengefasst: Web App <-> Broker Verbindung steht fest. Broker <-> ESP Verbindung sieht schlecht aus mit MQTT bis jetzt. Logik auf dem Broker ist noch nicht angefangen. Nächstes Ziel ist die Verbindung zum ESP, dann die Logik. Wenn wir das haben, geht es nur mehr um Quality of Life auf dem Broker. </br>
    Überlegswert ist, wohin die .dll Datei kommt zum Initialisieren eines neuen ESPs.
</br>
  - Martin 20231011
    - 2h: Mit Marco und Sebastian probiert eine MQTT Verbindung zwischen dem Broker und einem ESP herzustellen mit M2MQTT und MQTTnet, aber es ist uns nicht gelungen. Das Erkündigen geht weiter. Als nächstes werde ich persönlich auf M2MQTT auf dem Broker stellen, obwohl ich gesehen habe, dass man mit M2MQTT einen vorgefertigten, konfigurierbaren Broker benutzen soll(GnatMQ). GnatMQ will ich nicht benutzen, aber ich werde es mir anschauen.
</br>
  - Martin 20231012
    - 0.5h: Verbindung zwischen ein Desktop Client(mein Laptop mit "MQTTX" drauf) und unserem Broker auf dem RPi festgestellt. Das hat alles einwandfrei funktioniert, einfach mit der IP-Adresse des Brokers. MQTTX ist eine Applikation, die es mindestens für Linux gibt.
</br>
  - Martin 20231014
    - 4h: Programmierwerkstätte. 1h Netzwerkkonfiguration(debugging) ohne irgendeinem Resultat. "nmcli" funktioniert nur so halbwegs gut. </br>
    TCP Socket Verbindung zwischen RPi und ESP probiert. Die Verbindung funktioniert nur halbwegs, weil sie entweder nicht zur Stande kommt, aber sie unterbrochen wird nach ein paar Sekunden. Meinte Vermutung ist, dass das weg gehen wird, nachdem wir alles auf async umstellen. Vergleich und Probe eventuell am Mittwoch.
</br>
  - Mateusz 20231014 
    - 4h: Es wurde eine TCP-Verbindung mit dem ESP hergestellt, aber es treten Probleme auf, der Server und der ESP müssen gleichzeitig gestartet werden, um die Verbindung aufzubauen
  </br>
  - Mateusz 20231015 
    - 3h: Den Broker versucht, asynchron zu programmieren, jedoch bricht der Server die Verbindung zum Client beim Verbindungsaufbau ab.
</br>
  - Mateusz 20231017
    - 4h: Ich habe mein Projekt mit Martin's Projekt zusammengefasst. Darüber hinaus habe ich Refactoring-Arbeiten am gesamten Broker durchgeführt. Außerdem habe ich das Problem mit der Asynchronität gelöst, und das Problem mit dem gleichzeitigen Starten des ESP und des Servers hat sich durch die asynchrone Lösung ebenfalls erledigt.
</br>
  - Martin 20231017
    - 6.5h: Heute in der Früh das Demo-Projekt für TdoT asynchron gemacht und wollte die Verbindung testen, mit einem ESP, aber nach dem Flashen hatte ich ein Problem, was schon in der ESP Gruppe bekannt ist(ESP wir in VS nicht unter Device Explorer angezeigt). Später habe ich mir die Kommunikation zwischen Web App und Backend(Broker Logik) angeschaut. Ich weiß, bzw. spekuliere jetzt, dass die ganze Logik, die wir geschirben haben in die sogenannten "Controller" in dem ASP.Net Projekt reinkommen. Wir können weiterhin SOLID anwenden, aber der Ausgangspunkt liegt bei den Controllern. Dokumentation zu dem ASP.Net Bereich gibt es noch nicht, weil ich mich darin noch nicht auskenne, um irgendetwas korrektes zu schreiben.
</br>
  - Martin 20231018
    - 4h: Ich habe mich generell mit den anderen Mitgliedern über das Projekt unterhalten, mir Websockets und die Verbindung zwischen Web-App und Backend angeschaut. Jetzt habe ich genug Wissen, um die Controller schreiben zu können(hier hat Mateusz auch zugehört). Teilgenommen an ESP Lötung, sowie Testung mit dem RPi und ESP.
</br>
  - Mateusz 20231018
    - 4h: Obwohl ich größtenteils auf der Suche nach einer E14-Lampe war, habe ich schließlich doch noch Zeit mit Martin gefunden, um die Controller zu besprechen. Diese werden in Zukunft für die Kommunikation zwischen der Web-App und dem Broker sowie zwischen dem ESP und dem Broker verwendet.
</br>
  - Martin 20231020
    - 7h: Debugging mit Sebastian an der Kommunikation zwischen Web App und Broker Back End. Wir kame ständig an CORS-Errors oder an "Netzwerk"-Errors, obwohl es gar keine Netzwerktechnischen Problemen gab. Am Ende habe ich eine pure Javascript Datei geschrieben mit einem Fetch/Get-Request für die Temperatur und wir konnten mit Node.js als Server über die Logik des Brokers und an einem ESP gelangen, wo die Temperatur abgefragt wurde. Es gab eine sehr wichtige Zeile Code und das war folgende: </br>
    `process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";` </br>
    Heute war auch der erste TdoT und die Diplomarbeit wurde präsentiert, so gut wie es ging. Ich habe den Tag generell nur mit Debugging verbracht, aber jetzt haben wir am Ende eine Idee wie wir den "Fehler" beheben können. Meiner Meinung nach liegt es am Development Stack von Sebastian, weil ASP.Net und NextJS als Server nicht miteinander kommunizieren können.
</br>
  - Martin 20231021
    - 4h: Ich habe mich mit der MQTT Implementierung am Broker und auf den ESPs heute auseinandergesetzt. Mein hauptsächlicher Verdacht war und ist noch immer, dass die Kompatibilität der Zertifikaten zwischen MQTTnet und NanoFramework M2MQTT nicht gegeben ist. Das habe ich mir angeschaut, sowie das Erstellen eines X509 Zeritifikates, was noch möglich wäre, auszuprobieren. Die M2MQTT Dokumentation, sowie die MQTTnet Dokumentation habe ich mir durchgelesen und implementiert was wir brauchen. Die zwei Projekte, die in 'MQTT/20231021_DeleteAfterTest/' reinkommen, sind zwei Projekte, die von einem Mitglied unseres Projektes, der im Besitz eines ESPs ist, ausprobiert werden sollen. Falls wir hier keine positive Merkmale erkennen, sollten wir uns folgendes überlegen: </br>
      1. X509 Zeritifikat, in Komination mit SSL Zertifikat auf 'None' gesetzt. </br>
      2. Umstieg von MQTTnet auf etwas anderes, weil es kein Spielraum auf den ESPs gibt. </br>
</br>
Anbei ein Ausschnitt der MQTTnet Doku, die sich mit dem Server/Broker Teil des Libraries befasst: </br>
https://github.com/dotnet/MQTTnet/blob/master/Samples/Server/Server_ASP_NET_Samples.cs
</br>
  - Mateusz 20231031
      - 4h: 2x Meetings mit Martin wo wir unsere nächsten Schritte besprochen haben und die Statistics-Klasse und die SlothLogger-Klasse implementiert. Die Statistics-Klasse dient zum speichern von Messwerten in Dateien. Das schreiben von Werten in Dateien übernimmt die SlothLogger-Klasse, sowie das schreiben auf der Konsole.
</br>
  - Martin 20231031 & 20231105
    - 4h: Teams-Meetings mit Mateusz um die Entwicklung der Logik des Brokers zu besprechen. Wir haben in den Meetings hauptsächlich nur die kommenden Schritten besprochen. Was programmiert wurde ist in den Commits des Codes übersichtlich.
</br>
  - Mateusz 20231101 
    - 3h: Im Slothserver die Logik implementiert, das heißt unterscheiden zwischen einem StatusMessage und einem Message. Je nach Nachricht speichern wir sie in einer Datei oder leiten sie nur an den Webserver weiter. Die Statistic-Klasse um eine Methode erweitert die das auslesen von einer Datei ermöglicht.
</br>
  - Mateusz 20231102
    - 2h: Statistic umgebaut, die Liste wo die Daten gespeichert werden um zwei weitere Strings erweitert was uns das Speichern von Werten in Dateien erleichtert. Statistic zu einem Singleton gemacht, damit es zu keinen Fehlern im Laufe des Programms kommt.
</br>
  - Martin 20231117
    - 10h: Umstieg von RPi 3 auf RPi 4, Anpassung der Api beim Backend Projekt laut Post und Get Requests der WebApp, Netzwerkkonfigurationen am RPi. Api hat im Endeffekt funktioniert am Ende des Tages, nur bei der WebApp sind Probleme aufgetaucht mit Syntax Errors am Ende.
  - Martin 20231118
    - 5h: Kurze Austestung einer provisorischen WebApp, die Post Requests aufruft, um die Api auszutesten. Installation von Nginx und Reverse-Proxi damit aufgesetzt. Im Endeffekt Api aufrufbar nicht nur über localhost, sondern der IP-Adresse des RPis. Ansteuer möglich über Postman(damit wir die Api ansprechen können). Provisorische WebApp wurde geschmissen, da nutzlos.
  - Mateusz 20231118
    - 3h: Zusammenarbeit an dem Broker. Alles oben genannt von diesem Tag.
</br>
  - Martin 20231201
    - 2h: SQLite Recherche. Eine SQLite Datenbank wäre für uns sehr praktisch, da sie Leistung des Projekts sich deutlich verbessern würde. SQLite kann auf dem RPi tadellos laufen und wir können mit ein paar Tabellen alles speichern. Eine SQLite Datenbank wäre auch Thread-sicher, also bräuchten wir uns darüber auch keine Sorgen machen. </br>
    - sudo apt update
    - sudo apt install sqlite3
    - sqlite3 slothDatabase.db
    - CREATE TABLE IF NOT EXISTS table_name (Id INTEGER PRIMARY KEY, Time TEXT, Room TEXT, Value TEXT, Unit TEXT, Type TEXT);
    - Exit-Kommando: .quit
    - C# Package: Install-Package System.Data.SQLite
    - using System.Data.SQLite;
    - string connectionString = "Data Source=YourDatabaseFile.db;Version=3;";




