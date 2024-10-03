# Das Aufsetzen von .NET 7 auf dem Broker
    .NET 7 ist momentan die neueste stabile Version von .NET, weswegen wir sie verwenden. </br>

## Aufsetzen
    Angenommen, dass wir auf eigenen Rechnern entwickeln, brauchen wir auf dem Raspberry Pi nichts, außer die .NET Packages selbst und die Dependencies. </br>
    Dies ist leicht, wir brauchen nur </br>
    ` sudo apt-get install dotnet7` </br>
    laufen lassen und die Dependencies werden mitinstalliert. Hier mit dem sudo-Passwort Zugriff geben, nach Aufforderung und mit "y" bestätigen, auch nach Aufforderung. </br>
    Zum Entwickeln braucht man auf dem Raspberry Pi nichts installieren, da wir eben nur auf unseren eigenen Rechnern schreiben werden.
    </br></br>
    Mehr war es auf dem Raspberry Pi auch nicht, da die Entwicklungsumgebung die auf dem stattfindet.
    </br></br>

## Projekt Umgebung
    Es wäre optimal, wenn man sich ein Directory nur für den Projekt anliegt, um alles einheitlich zu halten.

## Entwicklungsumgebung unter Windows
    Wenn man sich für VisualStudio 2019 oder 2022 entschiedet, muss mna sich dann von der [Microsoft Webseite](https://dotnet.microsoft.com/en-us/download/dotnet/7.0) herunterladen. Ich hatte mich für Visual Studio 2019 entschieden, aber nach der Installation der SDK habe ich realisiert, dass sich die nur für Visual Studio 2022 installiert hat. Ich habe dann Visual Studio 2022 verwendet und das ging einwandfrei.

## Ausführen
    Wenn man unter Windows ein Konsolen-Projekt anlegt und schreibt, mit .NET Core, muss man folgende Schritte folgen, um ein Projekt zum Raspberry Pi herüberzubringen: </br>
    1. Projekt anlegen und schreiben und Windows mit Visual Studio 2017, 2019 oder 2022.
    2. In der Leiste, wo man ein Projekt zum Debuggen laufen lassen kann, stellt man von "Debug" auf "Release" um.
    3. Im Solution Explorer findet man das Projekt selbst(nicht solution) und bei einem Rechts-Klick drauf, findet man "Publish...".
    4. Publish -> Ordner -> Ordner auswählen -> weiter bis zum Publish.
    5. Bei "Konfiguration(?)" noch auf dem Stift draufklicken und einstellen für welche Prozessoren es geschrieben wurde(?).
    6. Weiters kommt noch, weil ich mich nicht an die ganze Reihenfolge erinnern kann.

    10. Schlussendlich habe ich "netcat" verwendet, um die Dateien zum RPi zu kopieren.

    </br>
    Wie wir sehen, haben wir keine Möglichkeit, außer mit VSCode, eine direkte Entwicklungsumgebung auf dem Raspberry Pi aufzusetzen. Was ich hier beschrieben habe ist eine okay-e Option, finde ich. </br>
    Weiters kann man sich die Option anschauen, die Microsoft angeblich anbietet unter Visual Studio(normal), dass man extern Debuggen kann. Das heißt, dass die Dateien wahrscheinlich zu jedem Debug auf dem Raspberry Pi gespielt werden.