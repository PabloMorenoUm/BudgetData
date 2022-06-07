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
        stage('Stage 1') {
            steps {
                echo 'Hello world!' 
            }
        }
    }
}
