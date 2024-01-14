import { Form, Link, useLoaderData } from "react-router-dom";
import { getTenants } from "../tenants";
import './App.css';
import { Tenant } from "./Tenant";

function App() {
    const { tenants } = useLoaderData() as { tenants: Tenant[] };

    const contents = tenants.length === 0
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
                        <td><Link to={`tenants/${tenant.id}`}>Edit</Link></td>
                        <td>
                            <Form
                                method="delete"
                                action={`tenant/${tenant.id}`}
                            >
                                <button type="submit">Delete</button>
                            </Form>
                        </td>
                    </tr>
                )}
            </tbody>
        </table>;

    return (
        <div>
            <h1 id="tabelLabel">Tenants</h1>
            {contents}
            <Form
                method="post"
                action="tenant"
            >
                <label>
                    <span>Name</span>
                    <input
                    placeholder="Name"
                    aria-label="Name"
                    type="text"
                    name="name"
                    />
                </label>
                <label>
                    <span>Country</span>
                    <input
                    placeholder="Country"
                    aria-label="Country"
                    type="text"
                    name="country"
                    />
                </label>
                <button type="submit">Add</button>
            </Form>
        </div>
    );
}

export default App;

export async function loader() {
    const tenants: Tenant[] = await getTenants();
    return { tenants };
}