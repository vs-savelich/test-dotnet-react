import { Form, Params, redirect, useLoaderData } from "react-router-dom";
import { Tenant, TenantData, getTenant, updateTenant } from "../tenants";

export default function EditTenant() {
  const { tenant } = useLoaderData() as { tenant: Tenant };

  return (
    <Form method="put" id="tenant-form">
      <label>
        <span>Name</span>
        <input
          placeholder="Name"
          aria-label="Name"
          type="text"
          name="name"
          defaultValue={tenant.name}
        />
      </label>
      <label>
        <span>Country</span>
        <input
          placeholder="Country"
          aria-label="Country"
          type="text"
          name="country"
          defaultValue={tenant.country}
        />
      </label>
      <p>
        <button type="submit">Save</button>
        <button type="button">Cancel</button>
      </p>
    </Form>
  );
}

export async function loader({ params }: {params: Params<string>}) {
    const tenant = await getTenant(params.tenantId!)
    return { tenant };
}

export async function action({ params, request }: {params: Params<string>, request: Request}) {
    const formData = await request.formData();
    await updateTenant(params.tenantId!, Object.fromEntries(formData) as TenantData);
    return redirect("/");
  }