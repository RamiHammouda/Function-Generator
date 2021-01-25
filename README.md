# Function Generator
[![pipeline status](https://gitlab.rz.htw-berlin.de/softwareentwicklungsprojekt/wise2020-21/team8/badges/master/pipeline.svg)](https://gitlab.rz.htw-berlin.de/softwareentwicklungsprojekt/wise2020-21/team8/-/commits/master)

<p align="center">
  <img align="center" src="https://i.ibb.co/cXb3BVJ/Tab3.png" width="400">
</p>


**Function Generator** is Windows desktop application, which aims to help the lovely engineers in **GFaI e.V** to ***simulate*** the ***sensor signal*** for main software using in water jet cutting factory. 


# Motivation

In the process of developing a main software for water jet cutting factory (EAS Leistand 4.0), database plays an important role. It stores the communicating signal of all sensors in the factory, so that the main control system can analyze the sensor signal, knows state of devices… and issues proper actions. Therefore, we need to build a software to simulate all sensor signal. And **Function Generator** comes up with the answer :). This software helps to generate data for database, simulate the operation states of sensor from operating parts of factory.

# Screenshot
| ![](https://i.ibb.co/SPQT9xx/Image-002.png) |<img src="https://i.ibb.co/nsx8y4C/Image00.png" width="600" title = "Multiple Profile tab">|
:-------------------------:|:-------------------------:
|![](https://i.ibb.co/DC1RJnR/Image-004.png)  |![](https://i.ibb.co/Zxsz6nZ/Image-006.png)|

  
# Usage



# Technologystack:
## Tools:
- OS Windows 10
- MS Visual Studio 2019
## Build
Built on **Microsoft .Net Framework** 4.7.2 , **Language C#**

- **WPF** for UI

- **Scotplott** v4.0.42 for Data Visualization

- **MySQL** v8.0.21 for Database ([Required: MySQL for Visual Studio](https://dev.mysql.com/downloads/windows/visualstudio/) and [Add Server Source to Visual Studio](https://dev.mysql.com/doc/visual-studio/en/visual-studio-connection-server-explorer.html))

- **Newtonsoft** v12.0.3 for Json

- **Extended Wpf Toolkit 4.0.2** for TimePicker Control

- ~~**NUnit** v3.12.0~~ for Unit-Test (used Built-in MS Test instead)


## Test
### CI Testing
- Run on Gitlab CI (Windows Runner)

- ~~Chocolatey Package Manager~~

- ~~NUnit 3 Console Runner v3.11.1~~
### Automation Testing
- ~~NUnit 3 Test Adapter Extension~~
- Built-in MS Test instead

# Feature List
- [x] Create sensor signal for testing with one **Single** target corlumn on database
- [x] Creat **Multiple** sensor signal to simulate different using scenario
- [x] Realtime trigger signal data visualization
 <br/> <br/>
- [x] Trigger sending data manually per button (as default)
- [x] Trigger sending data at schedule time
- [x] Trigger sending data at random time in day (till end of day), automatically start/stop sending and so on.
 <br/> <br/>
- [x] Simulate **Sine, Sawtooth, Triangle, Square, Analog Random, Digital Random** signal
 <br/> <br/>
- [x] Input Data Validation
- [x] Binding frequency input field to maximum offset ist selectable
 <br/> <br/>
- [x] Add or Remove a new signal profile directly from UI
 <br/> <br/>
- [x] Send signal data (single profile/ multiple profiles) to MySQL-Server Database
- [x] Automatically fill in the emty column with latest/default value.
 <br/> <br/>
- [x] Connection to database ist changeable and testable
 <br/> <br/>
- [x] Support to view database directly
- [x] Viewing Database Windows is selectable: all database (200 latest values) or single column on database upon picking (none picking - all database)
 <br/> <br/>
- [x] Export signal profiles to JSON file
- [x] Import signal profiles from JSON file at Starting program
- [x] Export setting data to JSON file
- [x] Import setting data from JSON file at Starting program
 <br/> <br/>
- [x] Provide a reset button to set all values to default
- [ ] Broadcast signal to the Universe (release soon:) )
# License
**MIT** © **HTW Berlin - IngenieurInformatik WS20/21** - ***Team 8:*** **Viet Anh Hoang, Khac Hoa Le, Patrick Schmidt, Rami Hammouda.**
