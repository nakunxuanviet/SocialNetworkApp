import React, { useEffect, useState } from "react";
import axios from "axios";
import { Container } from "semantic-ui-react";
import "./style.css";
import { Activity } from "../models/activity";
import NavBar from "./NavBar";
import ActivityDashBoard from "../../features/activities/dashboard/ActivityDashBoard";

function App() {
  const [activities, setActivities] = useState<Activity[]>([]);

  useEffect(() => {
    axios.get("https://localhost:5001/api/v1.0/activities").then((response) => {
      setActivities(response.data.items);
    });
  }, []);

  return (
    <>
      <NavBar />
      <Container style={{ marginTop: "7em" }}>
        <ActivityDashBoard activities={activities} />
      </Container>
    </>
  );
}

export default App;
