# Base ASP.NET Core runtime image
FROM nexus.w.mcb.md:8095/docker-image/aspnet:8.0.20 AS base

# Create non-root user, set up directory with proper permissions
RUN mkdir -p /app/Logs && \
   chown -R 1000:1000 /app && \
   chmod -R 755 /app

# Switch to non-root user
USER 1000

# Set working directory and expose port
WORKDIR /app
EXPOSE 8080

# Start final stage using base image
FROM base AS final

# Ensure non-root user for final stage
USER 1000

# Set ASP.NET environment
ENV ASPNETCORE_ENVIRONMENT="Release"

# Copy application files with proper ownership and permissions
COPY /app/build /app

# Set working directory for application
WORKDIR /app

# Define entry point
ENTRYPOINT ["dotnet", "Host.dll"]
