# Function Generator
[![pipeline status](https://gitlab.rz.htw-berlin.de/softwareentwicklungsprojekt/wise2020-21/team8/badges/master/pipeline.svg)](https://gitlab.rz.htw-berlin.de/softwareentwicklungsprojekt/wise2020-21/team8/-/commits/master)

<p align="center">
  <img align="center" src="https://i.ibb.co/cXb3BVJ/Tab3.png" width="400">
</p>


**Function Generator** is Windows desktop application, which aims to help the lovely engineers in **GFaI GmbH** to ***simulate*** the ***sensor signal*** for main software using in water jet cutting factory. 


# Motivation

In the process of developing a main software for project water jet cutting factory (ES Leistand 4.0) database plays an important role. It stores the communicating signal of all sensors in the factory, so that the main control system can analyze the sensor's signal, knows state of devices… and issues proper actions. Therefore, we need to build a software to simulate all sensors' signal. And **Function Generator** comes up with the answer :). This software helps to generate data for database, simulate the operation states of sensor from operating parts of factory.

# Screenshot

  <img src="https://i.ibb.co/1f3XBB5/Whats-App-Image-2020-11-07-at-11-40-01.jpg" width="500" title = "first Tab"> <img src="https://i.ibb.co/PxYhhQX/Whats-App-Image-2020-11-07-at-11-40-01-1.jpg" width="505" title = "first Tab">
  
# Usage



# Technologystack:

Built on **Microsoft .Net Framework** 4.7.2 , **Language C#**

- **WPF** for UI

- **Scotplott** 4.4.2 for Data Visualization

- **MySQL** for Database

- **NewTonSoft** for Json

- **NUNIT** for Testing

...
# Feature List
- [x] Create sensor signal for testing with one **Single** target corlumn on database
- [x] Creat **Multiple** sensor signal to simulate different using scenario
- [x] Realtime signal data visualization

- [x] Simulate ***Sine*** signal
- [x] Simulate ***Sawtooth*** signal
- [x] Simulate ***Square*** signal
- [x] Simulate ***Triangle*** signal
- [x] Simulate ***Analog Random*** signal
- [x] Simulate ***Digital Random*** signal

- [ ] Input Data Validation

- [x] Add a new signal profile directly from UI
- [x] Delete a signal profile directly from UI

- [x] Export signal data to JSON file
- [x] Provide a reset button to set all value to default
# License
**MIT** ©***Team 8:*** **Viet Anh Hoang, Khac Hoa Le, Patrick Schmidt, Rami Hammouda.**
