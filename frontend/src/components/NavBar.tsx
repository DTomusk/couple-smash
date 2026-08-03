import AppBar from '@mui/material/AppBar';
import Toolbar from '@mui/material/Toolbar';

export default function NavBar() {
    return (
        <>
            <AppBar position="fixed" color="primary">
                <Toolbar sx={{ display: 'flex', justifyContent: 'space-between' }}>
                </Toolbar>
            </AppBar>
        </>
    )
}