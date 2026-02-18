import { useEffect } from "react";
import Heading from "./Heading";

function Header() {


    useEffect(() => {
        const intervalId = setInterval(() => {

            console.log("Checking for updates...");
        }, 3000); // Check every 3 seconds

        return () => {
            clearInterval(intervalId); // Clean up on unmount
        };
    },[]);

    return null;
}

export default Header;
