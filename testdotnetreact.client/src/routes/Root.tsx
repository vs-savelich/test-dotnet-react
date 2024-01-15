import { Form, Link, Outlet, useLoaderData } from "react-router-dom";
import { Tenant, deleteTenant, getTenants } from "../tenants.ts";
import './Root.css';

function Root() {
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
                            <Form method="delete">
                                <input name="id" defaultValue={tenant.id} hidden />
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
            <Outlet />
        </div>
    );
}

export default Root;

export async function loader() {
    const tenants: Tenant[] = await getTenants();
    return { tenants };
}

export async function action({ request }: {request: Request}) {
    const formData = await request.formData();
    const tenant = await deleteTenant(formData.get("id") as string);
    return { tenant };
  }