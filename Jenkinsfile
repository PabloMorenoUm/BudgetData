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
          git branch: 'master', credentialsId: 'pers-acc-tkn-2nd-usr-pwd', url: 'https://github.com/PabloMorenoUm/BudgetData'
        }
      }
      stage('Restore packages') {
        steps {
          bat "dotnet restore ${workspace}\\BudgetData.sln"
        }
      }
        stage('Stage 1') {
            steps {
                echo 'Hello world!' 
            }
        }
    }
}
