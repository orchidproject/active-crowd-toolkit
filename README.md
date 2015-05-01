# active-crowd-toolkit
Repository storing the code of the ActiveCrowdToolkit: Benchmarking active learning algorithms for crowdsourcing research

## How to run the toolkit?
Click on ActiveCrowdToolkit.exe. The main screen will pop up.

## How to run experiments from command line?
Open the command line, navigate to the folder containing the toolkit files and run CrowdsourcingModels.exe with the following input parameters:

- Parameter 1: Execution mode = *run / aggregate*
   - *run* = the default mode for running new experiments
   - *aggregate* = the mode for aggregating results files into a single csv files with mean and standard error of the runs

- Parameter 2 (only for run mode): Path to the input dataset in csv format <WorkerID, TaskID, WorkerLabel, GoldLabel (optional)>

- Parameter 3 (only for run mode): Task selection method = RT / ET
 - RT = Random task selection
 - ET = Entropy task selection

- Parameter 4 (only for run mode): Worker selection method = RW / BT
 - RW = Random worker selection
 - BT = Entropy worker selection

- Parameter 5 (only for run mode): Index of the starting run

- Parameter 6 (only for run mode): Index of the ending run

This will run all the available aggregation models (Majority vote, Vote distribution, Dawid&Skene, BCC, CBCC) and save the results in a default folder: *ResultsActiveLearningToolkit/RunX*  

Examples:

Execute all the models with ET and RW for 1 to 10 runs:

    CrowdsourcingModels.exe Datasets/WS-AMT.csv ET RW 1 10
