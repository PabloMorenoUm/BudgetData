def serviceName = "budget-data"
def now = new Date()
def timestamp = now.format("yyyyMMddHHmm", TimeZone.getTimeZone('Europe/Berlin'))
pipeline {
    environment {
        DOCKER_IMAGE_NAME = "${serviceName}"
        DOCKER_IMAGE_VERSION = "${timestamp}-${env.GIT_COMMIT}"
        NEXUS_USERNAME = "admin"
        NEXUS_PASSWORD = "admin123"
    }

    options {
        skipStagesAfterUnstable()
    }
    
    agent {
        docker {
            image 'dotnet-build-image'
            registryUrl "https://${env.NEXUS_REGISTRY}"
            registryCredentialsId 'nexus-credentials'
            args "--name ${serviceName}-dotnet-build --cpus=1 --memory=4g"
        }
    }

    triggers {
        pollSCM('H/5 * * * *')
    }

    stages {
        stage('Console output of basic informations') {
            steps {
                sh "cat /etc/issue"
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
                sh "dotnet restore ${workspace}/BudgetData.sln"
            }
        }
        stage('Build package') {
            steps {
                sh "dotnet build --no-restore"
            }
        }
        stage('Running unit tests') {
            steps {
                sh "dotnet add ${workspace}/BudgetDataTest/BudgetDataTest.csproj package JUnitTestLogger --version 1.1.0"
                sh "dotnet test ${workspace}/BudgetDataTest/BudgetDataTest.csproj --logger \"junit;LogFilePath=\"${WORKSPACE}\"/TestResults/1.0.0.\"${env.BUILD_NUMBER}\"/results.xml\" --configuration release --collect \"Code coverage\""
            }
        }
        stage('Publish') {
            steps {
                sh "dotnet publish -p:InformationalVersion=${env.DOCKER_IMAGE_NAME}:${env.DOCKER_IMAGE_VERSION} -c=Release"
            }
        }
        
        stage('Docker build') {
            when { branch 'master'}
            steps {
                sh "echo ${env.DOCKER_IMAGE_NAME}:${env.DOCKER_IMAGE_VERSION} > version.txt"
                sh "docker build -f Dockerfile.run -t ${env.NEXUS_REGISTRY}/${env.DOCKER_IMAGE_NAME}:${env.DOCKER_IMAGE_VERSION} ."
            }
        }

    }
}
