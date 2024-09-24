for /f "delims=" %%i in (version.txt) do set ver=%%i
set /A ver+=1
echo %ver% >> version.txt
 
D:
cd "D:\Projects\Najm\NNMotor"
dotnet "D:\Sonar\SonarScanner.MSBuild.dll" begin /k:"Najm:NajmNetUnified" /v:%ver% /d:sonar.exclusions=**/Files/*,**/logs/*,**\obj\*,**\bin\* /d:sonar.coverageReportPaths=".\sonarqubecoverage\SonarQube.xml"

dotnet build tests\NNMotor.ApplicationCore.Tests\NNMotor.ApplicationCore.Tests.csproj

dotnet test tests\NNMotor.ApplicationCore.Tests\NNMotor.ApplicationCore.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=cobertura /p:Include="[NNMotor.ApplicationCore*]*" /p:Exclude="[*Tests]*" /p:CoverletOutput=./TestResults/

reportgenerator "-reports:Tests\NNMotor.ApplicationCore.Tests\TestResults\coverage.cobertura.xml" "-targetdir:sonarqubecoverage" "-reporttypes:SonarQube"

dotnet "D:\Sonar\SonarScanner.MSBuild.dll" end 