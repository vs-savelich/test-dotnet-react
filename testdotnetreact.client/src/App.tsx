import { useEffect, useState } from 'react';
import './App.css';

type Tenant = {
    id: string;
    name: string;
    country: string;
}

function App() {
    const [tenants, setTenants] = useState<Tenant[]>();

    useEffect(() => {
        populateTenantsData();
    }, []);

    const contents = tenants === undefined
        ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
        : <table className="table table-striped" aria-labelledby="tabelLabel">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Country</th>
                </tr>
            </thead>
            <tbody>
                {tenants.map(tenant =>
                    <tr key={tenant.id}>
                        <td>{tenant.name}</td>
                        <td>{tenant.country}</td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">Tenants</h1>
            {contents}
        </div>
    );

    async function populateTenantsData() {
        const response = await fetch("tenant");
        const data = await response.json();
        setTenants(data);
    }
}

export default App;