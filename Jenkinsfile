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
        stage('Stage 1') {
            steps {
                echo 'Hello world!' 
            }
        }
    }
}
