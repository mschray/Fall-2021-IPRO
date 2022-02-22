import React from "react";

import styles from "./Header.module.scss"
import { ReactComponent as JebraSVG } from "assets/JebraLogov2Dark.svg";

import { Link } from "react-router-dom";
import Button from "@mui/material/Button";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import { StyledEngineProvider } from "@mui/material/styles"

const headersData = [
    {
        label: "Home",
        href: "/",
    },
    {
        label: "Options",
        href: "/options",
    },
    {
        label: "Login",
        href: "/login",
    },
    {
        label: "Play",
        href: "/play",
    },
];
<<<<<<< Updated upstream
=======
  
const useStyles = makeStyles(() => ({
    header: {
        backgroundColor: "#262F34",
        filter: "drop-shadow(4px 4px 1px rgba(0, 0, 0, 0.25))",
        borderRadius: "15px",
        display: "flex",
        flexFlow: "row wrap",
        position: "relative",
        height: "5rem",
        paddingLeft: "5rem",
        paddingRight: "5rem",
        justifyContent: "space-between",
        "@media (max-width: 900px)": {
            paddingLeft: "0rem",
            paddingRight: "2rem",
            justifyContent: "start",
        },
    },
    menuButton: {
        fontFamily: "Open Sans, sans-serif",
        fontWeight: 700,
        size: "18px",
        marginLeft: "38px",
    },
    toolbar: {
        display: "flex",
        justifyContent: "space-evenly",
    },
    drawerContainer: {
        padding: "20px 30px",
    },
}));
  
export default function Header() {
    const { header, menuButton, toolbar, drawerContainer } = useStyles();

    const [state, setState] = useState({
        mobileView: false,
        drawerOpen: false,
    });

    const { mobileView, drawerOpen } = state;

    useEffect(() => {
        const setResponsiveness = () => {
        return window.innerWidth < 900
            ? setState((prevState) => ({ ...prevState, mobileView: true }))
            : setState((prevState) => ({ ...prevState, mobileView: false }));
        };
>>>>>>> Stashed changes


const Header: React.FC = () => {
    const menuButtons = headersData.map(({ label, href }) => (
        <Button
            className={styles.menuButton}
            key={label}
            color="inherit"
            to={href}
            component={Link}
        >
            {label}
        </Button>
    ));

    return (
        <StyledEngineProvider injectFirst>
            <AppBar className={styles.header}>
                <JebraSVG className={styles.logo} />
                <Toolbar className={styles.toolbar}>
                    <div className={styles.menu}>
                        {menuButtons}
                    </div>
                </Toolbar>
            </AppBar>
        </StyledEngineProvider>
    );
};

export default Header;