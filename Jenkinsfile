def serviceName = "budget-data"
def now = new Date()
def timestamp = now.format("yyyyMMddHHmm", TimeZone.getTimeZone('Europe/Berlin'))
pipeline {
    agent any 
    options {
        skipStagesAfterUnstable()
    }
    stages {
        stage('Console output of basic informations') {
            steps {
                sh 'cat /etc/issue'
            }
        }
        stage ('Clean workspace') {
            steps {
                cleanWs()
            }
        }
        stage ('Git Checkout') {
            steps {
                git branch: 'master', credentialsId: 'pers-acc-tkn-2nd-usr-pwd', url: 'https://github.com/PabloMorenoUm/BudgetData'
            }
        }
        stage('Install dotnet packages') {
            steps {
                sh 'dotnet restore'
            }
        }
        stage('Build package') {
            steps {
                sh 'dotnet build --no-restore'
            }
        }
        stage('Running unit tests') {
            steps {
                sh "dotnet add ${workspace}/BudgetDataTest/BudgetDataTest.csproj package JUnitTestLogger --version 1.1.0"
                sh "dotnet test ${workspace}/BudgetDataTest/BudgetDataTest.csproj --logger \"junit;LogFilePath=\"${WORKSPACE}\"/TestResults/1.0.0.\"${env.BUILD_NUMBER}\"/results.xml\" --configuration release --collect \"Code coverage\""
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
        stage('Stage 1') {
            steps {
                echo 'Hello world!' 
            }
        }
    }
    post {
        failure{
            reportPipelineResultIfOfInterest("FAILURE")
            imageCleanUp()
        }
        success{
            reportPipelineResultIfOfInterest("SUCCESS")
            imageCleanUp()
        }
    }
}
