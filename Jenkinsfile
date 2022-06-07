pipeline { 
    agent any 
    options {
        skipStagesAfterUnstable()
    }
    stages {
      stage ('Clean workspace') {
        steps {
          cleanWs()
        }
      }
      stage ('Git Checkout') {
        steps {
          git branch: 'master', credentialsId: 'pers-acc-tkn-2nd-usr-pwd', url: 'https://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@github.com/PabloMorenoUm/BudgetData'
        }
      }
      stage('Restore packages') {
        steps {
          bat "dotnet restore ${workspace}\\BudgetData.sln"
        }
      }
      stage('Clean') {
        steps {
          bat "msbuild.exe ${workspace}\\BudgetData.sln /nologo /nr:false /p:platform=\"x64\" /p:configuration=\"release\" /t:clean"
        }
      }
      stage('Build') {
        steps {
          bat "msbuild.exe ${workspace}\\BudgetData.sln /nologo /nr:false  /p:platform=\"x64\" /p:configuration=\"release\" /p:PackageCertificateKeyFile=<path-to-certificate-file>.pfx /t:clean;restore;rebuild"
        }
      }
      stage('Running unit tests') {
        steps {
          bat "dotnet add ${workspace}/BudgetDataTest/BudgetDataTest.csproj package JUnitTestLogger --version 1.1.0"
          bat "dotnet test ${workspace}/BudgetDataTest/BudgetDataTest.csproj --logger \"junit;LogFilePath=\"${WORKSPACE}\"/TestResults/1.0.0.\"${env.BUILD_NUMBER}\"/results.xml\" --configuration release --collect \"Code coverage\""
          powershell '''
          $destinationFolder = \"$env:WORKSPACE/TestResults\"
          if (!(Test-Path -path $destinationFolder)) {New-Item $destinationFolder -Type Directory}
          $file = Get-ChildItem -Path \"$env:WORKSPACE/<path-to-Unit-testing-project>/<name-of-unit-test-project>/TestResults/*/*.coverage\"
          $file | Rename-Item -NewName testcoverage.coverage
          $renamedFile = Get-ChildItem -Path \"$env:WORKSPACE/<path-to-Unit-testing-project>/<name-of-unit-test-project>/TestResults/*/*.coverage\"
          Copy-Item $renamedFile -Destination $destinationFolder
          '''            
        }        
      }
      stage('Convert coverage file to xml coverage file') {
        steps {
          bat "<path-to-CodeCoverage.exe>\\CodeCoverage.exe analyze  /output:${WORKSPACE}\\TestResults\\xmlresults.coveragexml  ${WORKSPACE}\\TestResults\\testcoverage.coverage"
        }
      }
      stage('Generate report') {
        steps {
          bat "<path-to-ReportGenerator.exe>\\ReportGenerator.exe -reports:${WORKSPACE}\\TestResults\\xmlresults.coveragexml -targetdir:${WORKSPACE}\\CodeCoverage_${env.BUILD_NUMBER}
        }
      }
      stage('Publish HTML report') {
        steps {
          publishHTML(target: [allowMissing: false, alwaysLinkToLastBuild: false, keepAll: false, reportDir: 'CodeCoverage_${BUILD_NUMBER}', reportFiles: 'index.html', reportName: 'HTML Report', reportTitles: 'Code Coverage Report'])
        }
      }
      post {
        always {
          archiveArtifacts artifacts: '**/*.msix', followSymlinks: false
          junit "TestResults/1.0.0.${env.BUILD_NUMBER}/results.xml"
        }
      }
    }
}
