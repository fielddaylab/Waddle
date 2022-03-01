# penguinsVR

## Firebase Telemetry Events

### Change Log
1. Initial version

### Events

1. [submit_survey](#submit_survey)
2. [load_minigame](#load_minigame)

<a name="submit_survey"/>

#### submit_survey

Player submits a completed survey.

| Parameter | Description |
| --- | --- |
| log_version | Current logging version |
| responses | Survey questions and responses in format "question:response", comma delimited |
| time | Time taken to submit survey |

<a name="load_minigame"/>

#### load_minigame

Player loads a given minigame scene.

| Parameter | Description |
| --- | --- |
| log_version | Current logging version |
| minigame | The minigame being loaded |

| minigame |
| --- |
| ProtectTheNest |
| MatingDance |
