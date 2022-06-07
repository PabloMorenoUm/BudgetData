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
        stage('Stage 1') {
            steps {
                echo 'Hello world!' 
            }
        }
    }
}
